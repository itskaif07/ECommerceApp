using System.Net.Mail;
using System.Net;
using Microsoft.Extensions.Configuration;

namespace ECommerceApp.Helpers
{
    public class EmailService
    {
        private readonly string _smtpServer;
        private readonly string _smtpUsername;
        private readonly string _smtpPassword;
        private readonly bool _enableSsl;
        private readonly int _smtpPort;

        public EmailService(IConfiguration configuration)
        {
            var smtpSettings = configuration.GetSection("SmtpSettings");

            _smtpServer = smtpSettings["Server"];
            _smtpUsername = smtpSettings["Username"];
            _smtpPassword = smtpSettings["Password"];
            _enableSsl = bool.Parse(smtpSettings["EnableSsl"]);
            _smtpPort = int.Parse(smtpSettings["Port"]);
        }

        public async Task SendEmailAsync(string toEmail, string subject, string body)
        {
            var smtpClient = new SmtpClient()
            {
                Host = "smtp.gmail.com",
                Port = _smtpPort,
                EnableSsl = _enableSsl,
                Credentials = new NetworkCredential(_smtpUsername, _smtpPassword)
            };

            var mail = new MailMessage()
            {
                Subject = subject,
                From = new MailAddress(_smtpUsername),
                Body = body,
                IsBodyHtml = true
            };

            mail.To.Add(toEmail);

            await smtpClient.SendMailAsync(mail);
        }

    }
}
