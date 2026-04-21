
using System.Net;
using System.Net.Mail;
using Microsoft.AspNetCore.Identity.UI.Services;
namespace Sistema_Actividades_Tlahuac.Data.Seed
{
    public class EmailSender : IEmailSender
    {
        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            var smtp = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential("lilianagerardom@gmail.com", "xjgd ndxt bejs fzyj"),
                EnableSsl = true,
            };

            var mail = new MailMessage
            {
                From = new MailAddress("lilianagerardom@gmail.com", "Sistema Tlahuac"),
                Subject = subject,
                Body = htmlMessage,
                IsBodyHtml = true,
            };

            mail.To.Add(email);

            await smtp.SendMailAsync(mail);
        }
    }
}