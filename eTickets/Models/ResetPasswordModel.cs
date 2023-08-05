using System.ComponentModel.DataAnnotations;

namespace eTickets.Models
{
    public class ResetPasswordModel
    {
        [Required]
        public string Userid { get; set; }
        [Required]
        public string Token { get; set; }
        [Required,DataType(DataType.Password)]
        public string NewPassword { get; set; }
        
        [Required, DataType(DataType.Password)]
        [Compare("NewPassword")]
        public string Cnfnewpassword { get; set; }
    }
}
