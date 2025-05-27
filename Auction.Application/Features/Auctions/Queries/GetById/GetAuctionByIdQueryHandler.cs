using Auction.Application.Common.Models;
using Auction.Application.Common.Models.Vm.Auctions.GetById;
using Auction.Application.Interfaces;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Auction.Application.Features.Auctions.Queries.GetById
{
    class GetAuctionByIdQueryHandler(IAuctionContext dbContext, IMapper mapper, ICurrentUserService currentUserService) : IRequestHandler<GetAuctionByIdQuery, Result<GetAuctionByIdVm>>
    {


        public async Task<Result<GetAuctionByIdVm>> Handle(GetAuctionByIdQuery request, CancellationToken cancellationToken)
        {
            var auctionFromDb = await dbContext.Auctions
                .Include(a => a.Product)
                    .ThenInclude(p => p.Images)
                .Include(a => a.Creator)
                .Include(a => a.Participations)
                    .ThenInclude(p => p.User)
                .FirstOrDefaultAsync(a => a.Id.Equals(Guid.Parse(request.AuctionId)), cancellationToken);

            if (auctionFromDb == null)
                return Result<GetAuctionByIdVm>.BadRequest("Аукцион не найден");

            return Result<GetAuctionByIdVm>.Ok(mapper.Map<GetAuctionByIdVm>(auctionFromDb));
        }
    }
}
