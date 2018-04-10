using BlendedAdmin.Infrastructure;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
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
        private IOptions<MailOptions> _options;
        private readonly ILogger<EmailService> _logger;

        public EmailService(IOptions<MailOptions> options, ILogger<EmailService> logger)
        {
            _options = options;
            _logger = logger;
        }

        public Task SendEmailAsync(string email, string title, string body)
        {
            try
            {
                SmtpClient client = new SmtpClient("");
                client.EnableSsl = true;
                client.Host = _options.Value.SmtpHost;
                client.UseDefaultCredentials = false;
                client.Credentials = new NetworkCredential(_options.Value.SmtpUser, _options.Value.SmtpPassword);

                MailMessage mailMessage = new MailMessage();
                mailMessage.From = new MailAddress(_options.Value.SmtpFrom);
                mailMessage.To.Add(email);
                mailMessage.Subject = title;
                mailMessage.Body = body;
                mailMessage.IsBodyHtml = true;
                client.Send(mailMessage);
            } catch (Exception ex)
            {
                _logger.LogError(ex, "Sending email.");
            }
            return Task.CompletedTask;
        }
    }
}
