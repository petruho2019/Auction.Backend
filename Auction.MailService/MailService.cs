using Auction.Application.Common.Models.Dto.Notifications;
using Auction.Application.Interfaces;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;
using MimeKit;

namespace Auction.MailService
{
    public class MailService(IConfiguration configuration) : IMailService
    {
        public async Task SendMessage(MailMessageDto mailMessage)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Server", configuration["MailSettings:SenderMail"]));
            message.To.Add(new MailboxAddress("", mailMessage.MailTo));
            message.Body = new TextPart(MimeKit.Text.TextFormat.Plain)
            {
                Text = mailMessage.Message
            };

            using (var client = new SmtpClient())
            {
                await client.ConnectAsync(configuration["MailSettings:SmtpServer"], 465, MailKit.Security.SecureSocketOptions.StartTls);
                await client.AuthenticateAsync(configuration["MailSettings:SenderMail"], configuration["MailSettings:SmtpPassword"]);
                await client.SendAsync(message);
            }
        }
    }
}
