using Auction.Application.Common.Models.Dto.Notifications;

namespace Auction.Application.Interfaces
{
    public interface IMailService
    {
        Task SendMessage(MailMessageDto mailMessage);
    }
}
