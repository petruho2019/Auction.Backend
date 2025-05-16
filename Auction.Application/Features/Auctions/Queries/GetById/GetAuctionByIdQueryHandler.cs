using Auction.Application.Common.Models;
using Auction.Application.Common.Models.Vm.Auctions.GetById;
using Auction.Application.Interfaces;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auction.Application.Features.Auctions.Queries.GetById
{
    class GetAuctionByIdQueryHandler : BaseComponentHandler, IRequestHandler<GetAuctionByIdQuery, Result<GetAuctionByIdVm>>
    {
        public GetAuctionByIdQueryHandler(IAuctionContext dbContext, IMapper mapper, ICurrentUserService currentUserService) : base(dbContext, mapper, currentUserService)
        {
        }

        public async Task<Result<GetAuctionByIdVm>> Handle(GetAuctionByIdQuery request, CancellationToken cancellationToken)
        {
            var auctionFromDb = await _dbContext.Auctions
                .Include(a => a.Product)
                    .ThenInclude(p => p.Images)
                .Include(a => a.Creator)
                .Include(a => a.Participations)
                    .ThenInclude(p => p.User)
                .FirstOrDefaultAsync(a => a.Id.Equals(Guid.Parse(request.AuctionId)), cancellationToken);

            if (auctionFromDb == null)
                return CreateFailureResult<GetAuctionByIdVm>("Аукцион не найден");

            return CreateSuccessResult(_mapper.Map<GetAuctionByIdVm>(auctionFromDb));
        }
    }
}
