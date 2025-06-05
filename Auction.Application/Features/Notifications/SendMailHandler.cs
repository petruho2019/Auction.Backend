using Auction.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace Auction.Application.Features.Notifications
{
    public class SendMailHandler(IMailService mailService, IAuctionContext context) : INotificationHandler<SendMailNotification>
    {
        public async Task Handle(SendMailNotification notification, CancellationToken cancellationToken)
        {
            notification.Auction.Product = await context.Products.Where(p => p.Id.Equals(notification.Auction.ProductId)).SingleAsync();
            notification.Auction.Buyer = await context.Users.Where(u => u.Id.Equals(notification.Auction.BuyerId)).SingleAsync();

            var message = MakeMessage(notification.Auction);

            await mailService.SendMessage(new()
            {
                MailTo = notification.Auction.Buyer.Email,
                Message = message
            });
        }

        private string MakeMessage(Domain.Models.Auction auction)
        {
            var message = new StringBuilder();

            message.Append("Победа в аукционе");
            message.AppendLine();
            message.Append($"Аукцион: '{auction.Id}'");
            message.AppendLine();
            message.Append($"Продукт: {auction.Product.Name}, {auction.Product.Description}, в количестве {auction.Quantity}, на сумму {auction.CurrentPrice}");
            message.AppendLine();
            message.Append("Поздравляем!");

            return message.ToString();
        }
    }
}
