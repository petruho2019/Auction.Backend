using Auction.Application.Common.Models;
using Auction.Application.Common.Models.Vm.AuctionParticipation.Create;
using MediatR;

namespace Auction.Application.Features.AuctionParticipstions.Commands
{
    public record CreateAuctionParticipationCommand : IRequest<Result<AuctionParticipationMakeABigVm>>
    {
        public string AuctionId { get; set; }
        public double BidPrice { get; set; }
    }
}
