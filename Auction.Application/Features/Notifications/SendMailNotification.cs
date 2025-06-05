using MediatR;

namespace Auction.Application.Features.Notifications
{
    public class SendMailNotification : INotification
    {
        public Domain.Models.Auction Auction { get; set; }
    }
}
