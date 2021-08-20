using System;
using System.Threading.Tasks;
using KristaShop.Common.Models;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;
using MimeKit.Utils;
using Module.Common.Business.Interfaces;
using Serilog;

namespace Module.Common.Business.Services {
    public class EmailService : IEmailService {
        private readonly EmailsSetting _emailSettings;
        private readonly ILogger _logger;

        public EmailService(IOptions<EmailsSetting> options, ILogger logger) {
            _emailSettings = options.Value;
            _logger = logger;
        }

        public async Task SendEmailAsync(EmailMessage emailMessage, string recipientName) {
            try {
                var mimeMessage = new MimeMessage();
                mimeMessage.From.Add(new MailboxAddress(_emailSettings.SenderName, _emailSettings.FromEmailAddress));
                mimeMessage.To.Add(new MailboxAddress(emailMessage.ToEmailAddress, emailMessage.ToEmailAddress));
                mimeMessage.Subject = emailMessage.Subject;
                mimeMessage.Importance = MessageImportance.High;
                mimeMessage.MessageId = MimeUtils.GenerateMessageId();
                mimeMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html) { Text = $"<h4>Уважаемый {recipientName},</h4><br>" + emailMessage.Content };

                using var client = new SmtpClient();
                await client.ConnectAsync(_emailSettings.Host, _emailSettings.Port, _emailSettings.EnableSSL);
                await client.AuthenticateAsync(_emailSettings.Login, _emailSettings.Password);
                await client.SendAsync(mimeMessage);
                await client.DisconnectAsync(true);
            } catch (Exception ex) {
                _logger.Error(ex, "Failed to send order email to client ({email}; {subject}). {message}", emailMessage.ToEmailAddress, emailMessage.Subject, ex.Message);
            }
        }
        
    }
}