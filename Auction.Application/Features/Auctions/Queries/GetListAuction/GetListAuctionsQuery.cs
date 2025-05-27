using Auction.Application.Common.Models.Vm.Auctions.GetList;
using MediatR;

namespace Auction.Application.Features.Auctions.Queries.GetListAuction
{
    public record GetListAuctionsQuery : IRequest<List<AuctionVm>>;
}
