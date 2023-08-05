using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace eTickets.Data.ViewModels
{
    public class RegisterVM
    {
        [Display(Name = "Full name")]
        [Required(ErrorMessage = "Full name is required")]
        public string FullName { get; set; }
        [Display(Name = "User name")]
        [Required(ErrorMessage = "Username is required")]
        public string UserName { get; set; }

        [Display(Name = "Email address")]
        [Required(ErrorMessage = "Email address is required")]
        [DataType(DataType.EmailAddress)]
        public string EmailAddress { get; set; }
        [Display(Name = "New password")]
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Confirm password")]
        [Required(ErrorMessage = "Confirm password is required")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Passwords do not match")]
        public string ConfirmPassword { get; set; }
        [Display(Name = "Phone Number")]
        [Required(ErrorMessage = "Phone number is required")]
        public string PhoneNumber { get; set; }
        public bool? EmailConfirmed { get; set; }
        [Display(Name = "Old password")]
        [Required]
        [DataType(DataType.Password)]
        public string OldPassword{ get; set; }




    }
}
