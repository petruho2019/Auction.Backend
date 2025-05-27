using Auction.Application.Common.Models;
using Auction.Application.Common.Models.Vm.Auctions.GetById;
using MediatR;

namespace Auction.Application.Features.Auctions.Queries.GetById
{
    public record GetAuctionByIdQuery : IRequest<Result<GetAuctionByIdVm>>
    {
        public string AuctionId { get; set; }
    }
}
