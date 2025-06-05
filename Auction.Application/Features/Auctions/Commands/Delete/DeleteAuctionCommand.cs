using Auction.Application.Common.Models;
using MediatR;

namespace Auction.Application.Features.Auctions.Commands.Delete
{
    public class DeleteAuctionCommand : IRequest<Result>
    {
        public Guid AuctionId { get; set; }
    }
}
