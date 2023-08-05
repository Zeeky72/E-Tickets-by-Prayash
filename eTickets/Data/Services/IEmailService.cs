using eTickets.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace eTickets.Data.Services
{
    public interface IEmailService
    {
        Task SendEmail(UserEmailOptions userEmailOptions);
        Task SendTestEmail(UserEmailOptions userEmailOptions);
        Task SendEmailForForgotPassword(UserEmailOptions userEmailOptions);
        Task SendBookingConfirmationEmail(UserEmailOptions userEmailOptions, string userEmail, string username, string seat, string movie, string price);


    }
}