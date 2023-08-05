using eTickets.Data;
using eTickets.Data.Services;
using eTickets.Data.Static;
using eTickets.Data.ViewModels;
using eTickets.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;


namespace eTickets.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly AppDbContext _context;
        private readonly IEmailService _emailService;
        private readonly IConfiguration _configuration;

        // private readonly EmailService _emailService;
        //Prayash sb-qtkbs26911783@personal.example.com
        //Nikhil sb-43urr4326871245@personal.example.com
        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, AppDbContext context, IEmailService emailService, IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
            _emailService = emailService;
            _configuration = configuration;
        }


        public async Task<IActionResult> Users()
        {
            var users = await _context.Users.ToListAsync();
            return View(users);
        }
        [HttpPost]
        public async Task<IActionResult> Users(string userId, bool newStatus)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user != null)
            {
                user.EmailConfirmed = newStatus;
                var updateResult = await _userManager.UpdateAsync(user);

                if (updateResult.Succeeded)
                {
                    // Redirect back to the original page (the list of users)
                    var userss = await _context.Users.ToListAsync();
                     user = await _userManager.FindByIdAsync(userId);
                    return RedirectToAction("Users", "Account");
                }
            }

            TempData["Error"] = "Failed to update EmailConfirmed status.";
            var users = await _context.Users.ToListAsync();
            return View(users);
        }

        public async Task<IActionResult> UsersUpdate()
        {
            var UID= _userManager.GetUserId(User);
            var user = await _userManager.FindByIdAsync(UID);
            var registerVM = new RegisterVM()
            {
                FullName = user.FullName, 
                EmailAddress = user.Email,
                UserName = user.UserName,
                PhoneNumber = user.PhoneNumber,
              //  EmailConfirmed = true
            };
            return View(registerVM);
        }
        [HttpPost]
        public async Task<IActionResult> UsersUpdate(RegisterVM registerVM)
        {
            if (!ModelState.IsValid) return View(registerVM);

            //var user = await _userManager.FindByEmailAsync(registerVM.EmailAddress);
            //if (user != null)
            //{
            //    TempData["Error"] = "This email address is already in use";
            //    return View(registerVM);
            //}
            //var userNam = await _userManager.Users.Where(x => x.UserName == registerVM.UserName).FirstOrDefaultAsync();
            //if (userNam != null)
            //{
            //    TempData["Error"] = "Username is already taken...";
            //    return View(registerVM);
            //}
            //if (registerVM.UserName == null || !(registerVM.UserName is string username))
            //{
            //    TempData["Error"] = "Username cannot be null";
            //    return View(registerVM);
            //}

            //// Check if the username contains any whitespace (tabspace)
            //if (registerVM.UserName.Contains(" ") || registerVM.UserName.Contains("\t"))
            //{
            //    TempData["Error"] = "Usename cannot have tabspace";
            //    return View(registerVM);
            //}
            //if (registerVM.UserName.Length >= 12 || registerVM.UserName.Length <= 6)
            //{
            //    TempData["Error"] = "username should have length of minimum 6 and maximum 12";
            //    return View(registerVM);
            //}
            foreach (char c in registerVM.FullName)
            {
                if (char.IsDigit(c))
                {
                    TempData["Error"] = "Name cannot have digits on it";
                    return View(registerVM);
                }
            }
            foreach (char c in registerVM.PhoneNumber)
            {
                if (char.IsLetter(c))
                {
                    TempData["Error"] = "Invalid phone number";
                    return View(registerVM);
                }
            }
            if (registerVM.PhoneNumber.Length != 10)
            {
                TempData["Error"] = "Invalid phone number";
                return View(registerVM);
            }

            var user = await _userManager.FindByEmailAsync(registerVM.EmailAddress);

            if (user != null)
            {
                // Update the properties of the existing user
                user.FullName = registerVM.FullName;
                user.UserName = registerVM.UserName;
                user.PhoneNumber = registerVM.PhoneNumber;
                user.EmailConfirmed = true;

                // Save the changes to the user
                var updateUserResponse = await _userManager.UpdateAsync(user);

                if (updateUserResponse.Succeeded)
                {
                    // User update succeeded, now update the password
                    var passwordCheck = await _userManager.CheckPasswordAsync(user, registerVM.OldPassword);

                    if (passwordCheck)
                    {
                      
                        var updateUserPasswordResponse = await _userManager.ChangePasswordAsync(user, registerVM.OldPassword, registerVM.Password);
                        if (!updateUserPasswordResponse.Succeeded)
                        {
                            TempData["Error"] = "Password must contain one capital letter, one alphanumeric, and one number, and must be at least 8 characters long.";
                            return View(registerVM);
                        }
                        await _signInManager.SignOutAsync();
                        return View("UpdateSuccessfull");
                    }
                    else
                    {
                        TempData["Error"] = "Wrong Old Password. Please, try again!";
                        return View(registerVM);
                    }

                    
                }
                else
                {
                    TempData["Error"] = "Failed to update user.";
                    return View(registerVM);
                }
            }
            else
            {
                TempData["Error"] = "User not found with the provided email address.";
                return View(registerVM);
            }
          

        }
        public async Task<IActionResult> UsersEmailUpdate()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return NotFound();
            }
            TempData["Error"] = "Your email must be unique and proper";
            return View(user);
        }
        [HttpPost]
        public async Task<IActionResult> UsersEmailUpdate(string id, [Bind("Id,Email")] ApplicationUser user)
        {
            if (id != user.Id)
            {
                return NotFound();
            }

            // Validate email format using regular expression
            string emailPattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
            if (!Regex.IsMatch(user.Email, emailPattern))
            {
                ModelState.AddModelError("Email", "Invalid email address format.");
                return View(user);
            }

            if (ModelState.IsValid)
            {
                var existingUser = await _userManager.FindByEmailAsync(user.Email);
                if (existingUser != null && existingUser.Id != user.Id)
                {
                    ModelState.AddModelError("Email", "This email address is already in use.");
                    return View(user);
                }

                var userToUpdate = await _userManager.FindByIdAsync(user.Id);
                if (userToUpdate != null)
                {
                    userToUpdate.Email = user.Email;

                    var updateResult = await _userManager.UpdateAsync(userToUpdate);
                    if (updateResult.Succeeded)
                    {
                        await _signInManager.SignOutAsync();
                        return View("UpdateSuccessfull");
                    }
                    else
                    {
                        // Failed to update user
                        TempData["Error"] = "Failed to update user.";
                        return View(user);
                    }
                }
                else
                {
                    TempData["Error"] = "User not found with the provided ID.";
                    return View(user);
                }
            }

            // Model validation failed
            return View(user);
        }


        private bool UserExists(string id)
        {
            return _context.Users.Any(e => e.Id == id);
        }
        public IActionResult Login() => View(new LoginVM());

        [HttpPost]
        public async Task<IActionResult> Login(LoginVM loginVM)
        {
            if (!ModelState.IsValid) return View(loginVM);
            var un = await _userManager.FindByEmailAsync(loginVM.EmailAddress);
          

            var user = await _userManager.FindByEmailAsync(loginVM.EmailAddress);
            if (user != null)
            {
                string username = un.UserName;
                string password = un.PasswordHash;
                if (user.EmailConfirmed == false)
                {
                    TempData["Error"] = "We have blocked your account for violations of company policy";
                    return View(loginVM);
                }
                var passwordCheck = await _userManager.CheckPasswordAsync(user, loginVM.Password);
                if (passwordCheck)
                {
                    var result = await _signInManager.PasswordSignInAsync(user, loginVM.Password, false, false);
                    if (result.Succeeded)
                    {
                       
                       
                            return RedirectToAction("Index", "Movies");
                        
                       
                    }
                }
                TempData["Error"] = "Wrong credentials. Please, try again!";
                return View(loginVM);
            }

            TempData["Error"] = "Wrong credentials. Please, try again!";
            return View(loginVM);
        }


        public IActionResult Register() => View(new RegisterVM());

        [HttpPost]
        public async Task<IActionResult> Register(RegisterVM registerVM)
        {
            string emailPattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
            if (!Regex.IsMatch(registerVM.EmailAddress, emailPattern))
            {
                ModelState.AddModelError("EmailAddress", "Invalid email address format.");
                return View(registerVM);
            }
            ModelState.Clear();
            if (!ModelState.IsValid) return View(registerVM);

            var user = await _userManager.FindByEmailAsync(registerVM.EmailAddress);
            if (user != null)
            {
                ModelState.AddModelError("EmailAddress", "This email address is already in use");
                return View(registerVM);
            }

            var userNam = await _userManager.Users.Where(x => x.UserName == registerVM.UserName).FirstOrDefaultAsync();
            if (userNam != null)
            {
                ModelState.AddModelError("UserName", "Username is already taken...");
                return View(registerVM);
            }

            if (registerVM.UserName == null || !(registerVM.UserName is string username))
            {
                TempData["Error"] = "Username cannot be null";
                return View(registerVM);
            }

            // Check if the username contains any whitespace (tabspace)
            if (registerVM.UserName.Contains(" ") || registerVM.UserName.Contains("\t"))
            {
                TempData["Error"] = "Username cannot have tabspace";
                return View(registerVM);
            }
            if (registerVM.UserName.Length >= 12 || registerVM.UserName.Length <= 6)
            {
                TempData["Error"] = "username should have length of minimum 6 and maximum 12";
                return View(registerVM);
            }
            foreach (char c in registerVM.FullName)
            {
                if (char.IsDigit(c))
                {
                    TempData["Error"] = "Name cannot have digits on it";
                    return View(registerVM);
                }
            }
            foreach (char c in registerVM.PhoneNumber)
            {
                if (char.IsLetter(c))
                {
                    TempData["Error"] = "Invalid phone number";
                    return View(registerVM);
                }
            }
            if (registerVM.PhoneNumber.Length != 10)
            {
                TempData["Error"] = "Invalid phone number";
                return View(registerVM);
            }

            var newUser = new ApplicationUser()
            {
                FullName = registerVM.FullName,
                Email = registerVM.EmailAddress,
                UserName = registerVM.UserName,
                PhoneNumber = registerVM.PhoneNumber,
                EmailConfirmed = true
            };

            var newUserResponse = await _userManager.CreateAsync(newUser, registerVM.Password);
            if (newUserResponse.Succeeded)
            {
                try
                {
                    if(newUser != null)
                    {
                        return View("RegisterCompleted");
                    }
                    await _userManager.AddToRoleAsync(newUser, UserRoles.User);
                    return View("RegisterCompleted");
                }
                catch (Exception ex)
                {
                    TempData["Exce"] = ex.Message.ToString();
                    return View("RegisterCompleted");
                }
            }
            else
            {
                // Clear model state errors before showing password error message
                ModelState.Clear();
                ModelState.AddModelError("Password", "Password must contain one capital letter, one alphanumeric, and one number, and must be at least 8 characters long.");
                return View(registerVM);
            }
        }


        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Movies");
        }

        public IActionResult AccessDenied(string ReturnUrl)
        {
            return View();
        }

        public IActionResult forgotPassword()
        {

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> forgotPassword(ForgotPasswordVM forgotPasswordVM)
        {
            var user = await _userManager.FindByEmailAsync(forgotPasswordVM.EmailAddress);

            if (!ModelState.IsValid) return View(forgotPasswordVM);
            if (ModelState.IsValid)
            {
                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                if (!string.IsNullOrEmpty(token))
                {
                    string appDomain = _configuration.GetSection("Application:AppDomain").Value;
                    string cnfLink = _configuration.GetSection("Application:ForgotPassword").Value;

                    UserEmailOptions options = new UserEmailOptions
                    {
                        ToEmails = new List<string>() { user.Email },
                        placeholders = new List<KeyValuePair<string, string>>()
                        {
                            new KeyValuePair<string, string>("{{UserName}}", user.FullName),
                            new KeyValuePair<string, string>("{{link}}",string.Format(appDomain+cnfLink,user.Id,token))
                        }
                    };
                    await _emailService.SendEmailForForgotPassword(options);

                    ModelState.Clear();
                    forgotPasswordVM.mailFlag = true;
                }
                // var accountDetails =await _userManager.FindByEmailAsync(forgotPasswordVM.EmailAddress);


                return View(forgotPasswordVM);
            }
            return View(forgotPasswordVM);

        }
        [HttpGet("reset-password")]
        public IActionResult resetpassword(string uid,string token)
        {
            ResetPasswordVM resetPasswordVM = new ResetPasswordVM
            {
                Token = token,
                Userid = uid
            };
            return View(resetPasswordVM);
        }
        [HttpPost("reset-password")]
        public async Task<IActionResult> resetpassword(ResetPasswordVM resetPasswordVM)
        {
           resetPasswordVM.Token = resetPasswordVM.Token.Replace(' ', '+');
           var result= await _userManager.ResetPasswordAsync(await _userManager.FindByIdAsync(resetPasswordVM.Userid),resetPasswordVM.Token,resetPasswordVM.NewPassword);  
          
            if(result.Succeeded)
            {
                ModelState.Clear();
                resetPasswordVM.isUpdated = true;
                return View(resetPasswordVM);

            }
            else
            {
                TempData["Error"] = "we are not able to update password";
                return View();
            }
           

        }
    }
}