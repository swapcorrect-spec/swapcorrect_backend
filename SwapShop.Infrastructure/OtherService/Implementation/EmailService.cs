using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MimeKit;
using SwapShop.Domain.Dtos.Request.Mailing;
using SwapShop.Domain.OtherService.Interface;

namespace SwapShop.Infrastructure.OtherService.Implementation
{
    public class EmailService : IEmailServices
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<EmailService> _logger;

        public EmailService(IConfiguration configuration, ILogger<EmailService> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        public async Task SendEmailAsync(Message message)
        {
            var emailMessage = CreateEmailMessage(message);
            await SendAsync(emailMessage);
        }

        private MimeMessage CreateEmailMessage(Message message)
        {
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress("email", _configuration["EmailConfiguration:UserName"]));
            emailMessage.To.AddRange(message.To);
            emailMessage.Subject = message.Subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html) { Text = message.Content };
            return emailMessage;
        }

        private async Task SendAsync(MimeMessage mailMessage)
        {
            using var client = new SmtpClient();
            try
            {
                var host = _configuration["EmailConfiguration:Host"];
                var port = int.Parse(_configuration["EmailConfiguration:Port"]!);
                var username = _configuration["EmailConfiguration:UserName"];
                var password = _configuration["EmailConfiguration:Password"];

                // port 587 + StartTls is required in containerised/cloud environments
                await client.ConnectAsync(host, port, SecureSocketOptions.StartTls);
                client.AuthenticationMechanisms.Remove("XOAUTH2");
                await client.AuthenticateAsync(username, password);
                await client.SendAsync(mailMessage);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to send email to {Recipients}", mailMessage.To);
                throw;
            }
            finally
            {
                await client.DisconnectAsync(true);
            }
        }
    }
}
