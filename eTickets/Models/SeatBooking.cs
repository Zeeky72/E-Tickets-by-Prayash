using eTickets.Models;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class SeatBooking
{
    [Key]
    public int Id { get; set; }

   
    public int MovieId { get; set; }
    public virtual Movie Movie { get; set; }


    public string UserId { get; set; }
    public virtual ApplicationUser User { get; set; }

    [Required]
    [MaxLength(450)]
    public string SeatNumber { get; set; }

  
    public int RowNumber => int.Parse(SeatNumber.Split('-')[0]);
    public int ColumnNumber => int.Parse(SeatNumber.Split('-')[1]);
   

    // Add any other properties and navigation properties as needed.


}
