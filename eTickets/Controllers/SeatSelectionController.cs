using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using eTickets.Data;
using eTickets.Models;
using eTickets.Data.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json.Linq;
using eTickets.Data.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace eTickets.Controllers
{
    public class SeatSelectionController : Controller
    {
        private readonly AppDbContext _dbContext;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmailService _emailService;
        private readonly IMoviesService _moviesService;
        public SeatSelectionController(AppDbContext dbContext, IEmailService emailService, UserManager<ApplicationUser> userManager, IMoviesService moviesService)
        {
            _dbContext = dbContext;
            _userManager = userManager;
            _emailService = emailService;
            _moviesService = moviesService;
        }

        // GET: SeatSelection/SeatSelection
        public async Task<IActionResult> SeatSelection(int movieId, string userId, decimal price)
        { var item = await _moviesService.GetMovieByIdAsync(movieId);
            var viewModel = new SeatBookingViewModel
            {
                MovieId = movieId,
                UserId = userId,
                AvailableSeats = GetAvailableSeats(movieId),
                Price=price,
                MovieName=item.Name
            };

            return View(viewModel);
        }

        // Helper method to get available seats from the database based on the movieId.
        private List<SeatModel> GetAvailableSeats(int movieId)
        {
            // Fetch seat booking data from the database for the given movieId.
            List<SeatBooking> seatBookings = _dbContext.SeatBookings
                .Where(sb => sb.MovieId == movieId)
                .ToList();

            // Assuming you have the total number of rows and columns for the theater.
            int totalRows = 10; // Replace with the actual total number of rows.
            int totalColumns = 10; // Replace with the actual total number of columns.

            // Create a list to store the seat information.
            List<SeatModel> availableSeats = new List<SeatModel>();

            // Populate the seat information.
            for (char row = 'A'; row <= 'A' + totalRows - 1; row++)
            {
                for (int col = 1; col <= totalColumns; col++)
                {
                    // Check if the seat is booked based on the seatBookings data.
                    string seatNumber = $"{row}-{col}";
                    bool isBooked = seatBookings.Any(sb => sb.SeatNumber.Contains(seatNumber));

                    availableSeats.Add(new SeatModel
                    {
                        Row = row,
                        Column = col,
                        IsBooked = isBooked
                    });
                }
            }

            return availableSeats;
        }

        [Authorize]
         [HttpPost]
        public async Task<IActionResult> BookSeats(int movieId, string userId, string selectedSeatss, decimal price)
        {
            // Assuming you have the required DbContext and SeatBooking model.
            string[] selectedSeatValues = selectedSeatss.Split(',');
            // Get the list of available seats for the selected movie from the database.
            var availableSeats = GetAvailableSeats(movieId);
            int noOfSeats = selectedSeatValues.Length;
            decimal realPrice = price * noOfSeats;
            TempData["MovieId"] = movieId;
            TempData["noOfSeats"] = noOfSeats;
            TempData["realPrice"] = realPrice.ToString();
            // Process the selected seats and update their booking status.
            foreach (var selectedSeat in selectedSeatValues)
            {
                var seatParts = selectedSeat.Split('-');
                if (seatParts.Length == 2 && int.TryParse(seatParts[1], out int col))
                {
                    char row = seatParts[0][0];

                    // Check if the selected seat exists in the available seats list.
                    var seat = availableSeats.FirstOrDefault(s => s.Row == row && s.Column == col);
                    if (seat != null && !seat.IsBooked)
                    {
                        seat.IsBooked = true;
                    }
                }
            }

            // Save the updated seat booking data to the database.

            //foreach (var seat in availableSeats)
            //{
            //    if (seat.IsBooked)
            //    {
            //        var seatNumber = $"{seat.Row}-{seat.Column:D2}";
            //        _dbContext.SeatBookings.Add(new SeatBooking
            //        {
            //            MovieId = movieId,
            //            UserId = userId,
            //            SeatNumber = selectedSeatss
            //        });
            //    }
            //}

            //_dbContext.SaveChanges();

            var user = await _userManager.FindByIdAsync(userId);

            UserEmailOptions options = new UserEmailOptions
            {
                ToEmails = new List<string>() { user.Email},
                placeholders = new List<KeyValuePair<string, string>>()
                        {
                            new KeyValuePair<string, string>("{{UserName}}", user.FullName),
                           // new KeyValuePair<string, string>("{{link}}",string.Format(appDomain+cnfLink,user.Id,token))
                        }
            };
          //  var item = await _moviesService.GetMovieByIdAsync(movieId);

           // await _emailService.SendBookingConfirmationEmail(options,user.Email,user.FullName, selectedSeatss,item.Name, realPrice.ToString());
            //    return RedirectToAction("Confirmation", "SeatSelection");
            //   return RedirectToAction("AddItemToShoppingCart", "Orders", new { id = movieId, totalPrice = realPrice });
            return RedirectToAction("AddItemToShoppingCart", "Orders", new { id = movieId, totalPrice = realPrice, noOfSeats = noOfSeats,userId,selectedSeatss });
        }


        public IActionResult Confirmation()
        {
            return View();
        }
    }
}
