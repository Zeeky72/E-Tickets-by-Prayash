using eTickets.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.AspNetCore.Mvc;
using System;
using QRCoder;



namespace eTickets.Data.Services
{
    public class EmailService : IEmailService
    {
       
        private const string templatePath = @"EmailTemplate/{0}.html";
        private readonly SMTPConfigModel _smtpconfig;

        public async Task SendTestEmail(UserEmailOptions userEmailOptions)
        {
            userEmailOptions.subject = "Reset your password!!";
            userEmailOptions.body = updatePlaceHolders( GetEmailBody("TestEmail"),userEmailOptions.placeholders);

            await SendEmail(userEmailOptions);
        }
        public async Task SendEmailForForgotPassword(UserEmailOptions userEmailOptions)
        {
            userEmailOptions.subject = "Reset your password!!";
            userEmailOptions.body = updatePlaceHolders(GetEmailBody("ResetPassword"), userEmailOptions.placeholders);

            await SendEmail(userEmailOptions);
        }
        public async Task SendBookingConfirmationEmail(UserEmailOptions userEmailOptions,string userEmail, string username, string seat, string movie, string price)
        {
            userEmailOptions.subject = "Booking Confirmation!!";
            userEmailOptions.body = updatePlaceHolders(GetEmailBody("SeatBookingCnf"), userEmailOptions.placeholders);

            await SendEmailforcnf(userEmailOptions, userEmail, username, seat, movie,  price);

        }
        public async Task SendEmailforcnf(UserEmailOptions userEmailOptions, string userEmail, string username, string seat, string movie,string price)
        {
          
            byte[] pdfBytes = GeneratePDF(username, seat, movie, price);
            MailMessage mail = new MailMessage
            {
                Subject = userEmailOptions.subject,
                Body = userEmailOptions.body,
                From = new MailAddress(_smtpconfig.SenderAddress, _smtpconfig.SenderDisplayName),
                IsBodyHtml = _smtpconfig.IsBodyHTML,


            };
            foreach (var toEmail in userEmailOptions.ToEmails)
            {
                mail.Attachments.Add(new Attachment(new MemoryStream(pdfBytes), "E-ticket by Prayash.pdf", "application/pdf"));

                mail.To.Add(toEmail);
            }
            NetworkCredential networkCredential = new NetworkCredential(_smtpconfig.UserName, _smtpconfig.Password);
            SmtpClient smtpClient = new SmtpClient
            {
                Host = _smtpconfig.Host,
                Port = _smtpconfig.Port,
                EnableSsl = _smtpconfig.EnableSsl,
                UseDefaultCredentials = _smtpconfig.UseDefaultCredentials,
                Credentials = networkCredential
            };
            mail.BodyEncoding = Encoding.Default;
            await smtpClient.SendMailAsync(mail);
        }

        public EmailService(IOptions<SMTPConfigModel> smtpconfig)
        {
            _smtpconfig = smtpconfig.Value;
        }
        public async Task SendEmail(UserEmailOptions userEmailOptions)
        {
            MailMessage mail = new MailMessage
            {
                Subject = userEmailOptions.subject,
                Body = userEmailOptions.body,
                From = new MailAddress(_smtpconfig.SenderAddress, _smtpconfig.SenderDisplayName),
                IsBodyHtml = _smtpconfig.IsBodyHTML,
                

            };
            foreach (var toEmail in userEmailOptions.ToEmails)
            {
                mail.To.Add(toEmail);
            }
            NetworkCredential networkCredential = new NetworkCredential(_smtpconfig.UserName, _smtpconfig.Password);
            SmtpClient smtpClient = new SmtpClient
            {
                Host = _smtpconfig.Host,
                Port = _smtpconfig.Port,
                EnableSsl = _smtpconfig.EnableSsl,
                UseDefaultCredentials = _smtpconfig.UseDefaultCredentials,
                Credentials = networkCredential
            };
            mail.BodyEncoding = Encoding.Default;
            await smtpClient.SendMailAsync(mail);
        }
       
        private string GetEmailBody(string templateName)
        {
            var body = File.ReadAllText(string.Format(templatePath, templateName));
            return body;
        }
        public string updatePlaceHolders(string text,List<KeyValuePair<string, string>> keyValuePairs)
        {
            if(!string .IsNullOrEmpty(text) && keyValuePairs != null)
            {
                foreach(var placeholder in keyValuePairs)
                {
                    if (text.Contains(placeholder.Key))
                    {
                        text=text.Replace(placeholder.Key, placeholder.Value);
                    }
                }
            }
            return text;
        }

        public byte[] GeneratePDF(string username, string seat, string movie, string moviePrice)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                // Load the image from the URL or local path
                string imageUrl = "https://purepng.com/public/uploads/large/purepng.com-popcornpopcorncorndent-cornflint-corn-1411528651541prz2y.png";
                Image backgroundImage = Image.GetInstance(new Uri(imageUrl));

                // Document document = new Document(PageSize.A4, 20f, 20f, 20f, 20f);
                float screenWidthInPoints = 314; // Example width
                float screenHeightInPoints = 470;
                Document document = new Document(new Rectangle(screenWidthInPoints, screenHeightInPoints));

                PdfWriter writer = PdfWriter.GetInstance(document, ms);

                document.Open();

                // Scale the image to fit the page while maintaining aspect ratio
                backgroundImage.SetAbsolutePosition(0, 0);
                backgroundImage.ScaleToFit(document.PageSize.Width, document.PageSize.Height);
                PdfContentByte canvas = writer.DirectContentUnder;
                canvas.AddImage(backgroundImage);

                // Add "Your Ticket" text at the top in bigger font
                BaseFont titleBaseFont = BaseFont.CreateFont(BaseFont.HELVETICA_BOLD, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                Font titleFont = new Font(titleBaseFont, 32, Font.NORMAL, BaseColor.BLACK);
                Paragraph header = new Paragraph("Your Ticket", titleFont);
                header.Alignment = Element.ALIGN_CENTER;
                document.Add(header);

                // Add content in the center with italic font
                BaseFont contentBaseFont = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                Font contentFont = new Font(contentBaseFont, 18, Font.ITALIC, BaseColor.BLACK);
                Paragraph userParagraph = new Paragraph($"Name: {username}", contentFont);
                userParagraph.Alignment = Element.ALIGN_CENTER;
                document.Add(userParagraph);

                Paragraph seatParagraph = new Paragraph($"Seat Selected: {seat}", contentFont);
                seatParagraph.Alignment = Element.ALIGN_CENTER;
                document.Add(seatParagraph);

                Paragraph movieParagraph = new Paragraph($"Movie Selected: {movie}", contentFont);
                movieParagraph.Alignment = Element.ALIGN_CENTER;
                document.Add(movieParagraph);

                Paragraph priceParagraph = new Paragraph($"Movie Price: {moviePrice:C}", contentFont);
                priceParagraph.Alignment = Element.ALIGN_CENTER;
                document.Add(priceParagraph);

                // Generate the QR code with the provided details
                string qrCodeContent = $"Username: {username}, Seat: {seat}, Movie: {movie}, Price: {moviePrice:C}";
                QRCodeGenerator qrCodeGenerator = new QRCodeGenerator();
                QRCodeData qrCodeData = qrCodeGenerator.CreateQrCode(qrCodeContent, QRCodeGenerator.ECCLevel.Q);
                QRCode qrCode = new QRCode(qrCodeData);

                // Convert the QR code to a Bitmap image
                System.Drawing.Bitmap qrCodeImage = qrCode.GetGraphic(20);

                // Convert the Bitmap image to an iTextSharp image
                Image qrCodePdfImage = Image.GetInstance(qrCodeImage, System.Drawing.Imaging.ImageFormat.Jpeg);
                qrCodePdfImage.ScaleToFit(150, 150); // Adjust the size of the QR code image as needed

                // Add the QR code image to the PDF
                qrCodePdfImage.Alignment = Element.ALIGN_CENTER;
                document.Add(qrCodePdfImage);

                // Add "E-tickets by prayash" at the bottom
                BaseFont footerBaseFont = BaseFont.CreateFont(BaseFont.HELVETICA_BOLD, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                Font footerFont = new Font(footerBaseFont, 14, Font.NORMAL, BaseColor.BLACK);
                Paragraph footer = new Paragraph("© 2023 - eTickets by PRAYASH", footerFont);
                footer.Alignment = Element.ALIGN_CENTER;
                document.Add(footer);

                document.Close();

                return ms.ToArray();
            }
        }





    }
}
