using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace BlendedAdmin.Services
{
    public interface IEmailService
    {
        Task SendEmailAsync(string email, string title, string body);
    }

    public class EmailService : IEmailService
    {
        public Task SendEmailAsync(string email, string title, string body)
        {
            SmtpClient client = new SmtpClient("");
            client.UseDefaultCredentials = false;
            client.Credentials = new NetworkCredential("", "");

            MailMessage mailMessage = new MailMessage();
            mailMessage.From = new MailAddress("whoever@me.com");
            mailMessage.To.Add(email);
            mailMessage.Subject = title;
            mailMessage.Body = body;
            client.Send(mailMessage);
            return Task.CompletedTask;
        }
    }
}
