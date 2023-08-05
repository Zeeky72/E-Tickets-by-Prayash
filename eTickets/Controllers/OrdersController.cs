using eTickets.Data;
using eTickets.Data.Cart;
using eTickets.Data.Services;
using eTickets.Data.Static;
using eTickets.Data.ViewModels;
using eTickets.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace eTickets.Controllers
{
    [Authorize] 
    public class OrdersController : Controller
    {
        private readonly IMoviesService _moviesService;
        private readonly ShoppingCart _shoppingCart;
        private readonly IOrdersService _ordersService;
        private readonly AppDbContext _dbContext;
        private readonly IEmailService _emailService;
        private readonly UserManager<ApplicationUser> _userManager;


        public OrdersController(AppDbContext dbContext,IMoviesService moviesService, IEmailService emailService, ShoppingCart shoppingCart, IOrdersService ordersService, UserManager<ApplicationUser> userManager)
        {
            _moviesService = moviesService;
            _shoppingCart = shoppingCart;
            _ordersService = ordersService;
            _dbContext = dbContext;
            _emailService = emailService;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            string userRole = User.FindFirstValue(ClaimTypes.Role);

            var orders = await _ordersService.GetOrdersByUserIdAndRoleAsync(userId, userRole);
            return View(orders);
        }

        public IActionResult ShoppingCart(decimal totalPrice,int noOfSeats,int id, string userId,string selectedSeatss)
        {
            var items = _shoppingCart.GetShoppingCartItems();
            _shoppingCart.ShoppingCartItems = items;
            // TempData.TryGetValue("TotalPrice", out object totalPriceObj) && decimal.TryParse(totalPriceObj.ToString(), out decimal totalPrice);
            var response = new ShoppingCartVM()
            {
                ShoppingCart = _shoppingCart,
                ShoppingCartTotal = (double)totalPrice,
               NoOfSeats= noOfSeats,
               UserId= userId,
               MovieId= id,
               SelectedSeats= selectedSeatss
            };

            return View(response);
        }

        public async Task<IActionResult> AddItemToShoppingCart(int id,decimal totalPrice,int noOfSeats,string userid,string selectedSeatss)
        {
            
            var item = await _moviesService.GetMovieByIdAsync(id);

            if (item != null)
            {
                _shoppingCart.AddItemToCart(item,noOfSeats);
            }
          
           // return RedirectToAction(nameof(ShoppingCart));
            return RedirectToAction(nameof(ShoppingCart), new { totalPrice,noOfSeats,id,userid, selectedSeatss });
        }

        public async Task<IActionResult> RemoveItemFromShoppingCart(int id)
        {
            var item = await _moviesService.GetMovieByIdAsync(id);

            if (item != null)
            {
                _shoppingCart.RemoveItemFromCart(item);
            }
            return RedirectToAction(nameof(ShoppingCart));
        }

        public async Task<IActionResult> CompleteOrder(string userId, int movieId, string totalPrice,int noOfSeats, string selectedSeats)
        {
            var items = _shoppingCart.GetShoppingCartItems();
            string userIdd = User.FindFirstValue(ClaimTypes.NameIdentifier);
            string userEmailAddress = User.FindFirstValue(ClaimTypes.Email);

            await _ordersService.StoreOrderAsync(items, userIdd, userEmailAddress);
            await _shoppingCart.ClearShoppingCartAsync();
            _dbContext.SeatBookings.Add(new SeatBooking
            {
                MovieId = movieId,
                UserId = userId,
                SeatNumber = selectedSeats
            });
            _dbContext.SaveChanges();
            var user = await _userManager.FindByIdAsync(userId);

            UserEmailOptions options = new UserEmailOptions
            {
                ToEmails = new List<string>() { user.Email },
                placeholders = new List<KeyValuePair<string, string>>()
                        {
                            new KeyValuePair<string, string>("{{UserName}}", user.FullName),
                           // new KeyValuePair<string, string>("{{link}}",string.Format(appDomain+cnfLink,user.Id,token))
                        }
            };
             var item = await _moviesService.GetMovieByIdAsync(movieId);

            await _emailService.SendBookingConfirmationEmail(options,user.Email,user.FullName, selectedSeats, item.Name, totalPrice);

            return View("OrderCompleted");
        }
    }
}
