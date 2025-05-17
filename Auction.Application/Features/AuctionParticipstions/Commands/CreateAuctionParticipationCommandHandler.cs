using Auction.Application.Common.Models;
using Auction.Application.Common.Models.Vm.AuctionParticipation.Create;
using Auction.Application.Interfaces;
using Auction.Domain.Models;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auction.Application.Features.AuctionParticipstions.Commands
{
    public class CreateAuctionParticipationCommandHandler : BaseComponentHandler, IRequestHandler<CreateAuctionParticipationCommand, Result<AuctionParticipationMakeABigVm>>
    {
        public CreateAuctionParticipationCommandHandler(IAuctionContext dbContext, IMapper mapper, ICurrentUserService currentUserService) : base(dbContext, mapper, currentUserService)
        {
        }

        public async Task<Result<AuctionParticipationMakeABigVm>> Handle(CreateAuctionParticipationCommand request, CancellationToken cancellationToken)
        {
            if (!await _dbContext.Auctions.AnyAsync(a => a.Id.Equals(Guid.Parse(request.AuctionId)), cancellationToken))
                return CreateFailureResult<AuctionParticipationMakeABigVm>("Аукцион не найден");

            if (request.BidPrice < 0)
                return CreateFailureResult<AuctionParticipationMakeABigVm>("'Ставка' не может быть меньше либо равнятся 0");

            var auctionId = Guid.Parse(request.AuctionId);
            // TODO спросить про проверку, зачем тут проверять если 100%
            // уверен что цена валидна в auction current price
            var maxBidFromAuctionParticipations = await _dbContext.AuctionParticipations
                .Select(ap => (double?)ap.BidPrice) 
                .MaxAsync(cancellationToken);

            if (maxBidFromAuctionParticipations.HasValue && request.BidPrice <= maxBidFromAuctionParticipations.Value)
                return CreateFailureResult<AuctionParticipationMakeABigVm>("Введенная ставка не может быть меньше текущей цены");

            if (request.BidPrice <= await _dbContext.Auctions
                .Where(a => a.Id.Equals(auctionId))
                .MaxAsync(a => a.CurrentPrice, cancellationToken)
                )
                return CreateFailureResult<AuctionParticipationMakeABigVm>("Введенная ставка не может быть меньше текущей цены");

            var auctionParticipation = new AuctionParticipation()
            {
                Id = Guid.NewGuid(),
                AuctionId = auctionId,
                UserId = _currentUserService.UserId,
                BidPrice = request.BidPrice,
                BidTime = DateTime.UtcNow
            };

            var auctionFromDb = await _dbContext.Auctions.FirstOrDefaultAsync(a => a.Id.Equals(auctionId), cancellationToken)!;
            auctionFromDb!.CurrentPrice = request.BidPrice;

            await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == _currentUserService.UserId, cancellationToken: cancellationToken);

            await _dbContext.AuctionParticipations.AddAsync(auctionParticipation, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return CreateSuccessResult(_mapper.Map<AuctionParticipationMakeABigVm>(auctionParticipation));

        }
    }
}
