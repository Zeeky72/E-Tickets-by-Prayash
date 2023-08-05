using eTickets.Models;
using System.Collections.Generic;

namespace eTickets.Data.ViewModels
{
    public class SeatBookingViewModel
    {
        public int MovieId { get; set; }
        public string UserId { get; set; }
        public decimal Price { get; set; }
        public string SelectedSeats { get; set; }
        public List<SeatModel> AvailableSeats { get; set; }
        public string MovieName { get; set; }
    }

}
