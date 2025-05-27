using Auction.Application.Common.Models.Vm.Auctions.GetList;
using Auction.Application.Interfaces;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Auction.Application.Features.Auctions.Queries.GetListAuction
{
    public class GetListAuctionsQueryHandler(IAuctionContext dbContext, IMapper mapper, ICurrentUserService currentUserService) : IRequestHandler<GetListAuctionsQuery, List<AuctionVm>>
    {

        public async Task<List<AuctionVm>> Handle(GetListAuctionsQuery request, CancellationToken cancellationToken)
        {
            var listAuctions = await dbContext.Auctions
                .Include(a => a.Product)
                .Include(a => a.Creator)
                .Include(a => a.Product.Images)
                .ToListAsync(cancellationToken: cancellationToken);

            return mapper.Map<List<AuctionVm>>(listAuctions);
        }
    }
}
