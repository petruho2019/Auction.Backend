using Auction.Application.Common.Models;
using MediatR;

namespace Auction.Application.Features.Auctions.Commands.EndAction
{
    public record CompleteAuctionCommand : IRequest<Result>
    {
        public Guid AuctionId { get; set; }
    }
}
