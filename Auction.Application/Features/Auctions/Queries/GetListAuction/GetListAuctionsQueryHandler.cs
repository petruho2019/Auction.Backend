using Auction.Application.Common.Models.Vm.Auctions.GetList;
using Auction.Application.Interfaces;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Auction.Application.Features.Auctions.Queries.GetListAuction
{
    public class GetListAuctionsQueryHandler : BaseComponentHandler, IRequestHandler<GetListAuctionsQuery, List<AuctionVm>>
    {
        public GetListAuctionsQueryHandler(IAuctionContext dbContext, IMapper mapper, ICurrentUserService currentUserService) : base(dbContext, mapper, currentUserService)
        {
        }

        public async Task<List<AuctionVm>> Handle(GetListAuctionsQuery request, CancellationToken cancellationToken)
        {
            var listAuctions = await _dbContext.Auctions
                .Include(a => a.Product)
                .Include(a => a.Creator)
                .Include(a => a.Product.Images)
                .ToListAsync(cancellationToken: cancellationToken);

            return _mapper.Map<List<AuctionVm>>(listAuctions);
        }
    }
}
