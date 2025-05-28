using Auction.Application.Common.Models;
using Auction.Application.Common.Models.Vm.AuctionParticipation.Create;
using Auction.Application.Interfaces;
using Auction.Domain.Models;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Auction.Application.Features.AuctionParticipstions.Commands
{
    public class CreateAuctionParticipationCommandHandler(IAuctionContext dbContext, IMapper mapper, ICurrentUserService currentUserService) : IRequestHandler<CreateAuctionParticipationCommand, Result<AuctionParticipationMakeABigVm>>
    {

        public async Task<Result<AuctionParticipationMakeABigVm>> Handle(CreateAuctionParticipationCommand request, CancellationToken cancellationToken)
        {
            if (!await dbContext.Auctions.AnyAsync(a => a.Id.Equals(Guid.Parse(request.AuctionId)), cancellationToken))
                return Result<AuctionParticipationMakeABigVm>.BadRequest("Аукцион не найден");

            if (request.BidPrice < 0)
                return Result<AuctionParticipationMakeABigVm>.BadRequest("'Ставка' не может быть меньше либо равнятся 0");

            var auctionId = Guid.Parse(request.AuctionId);

            // TODO спросить про проверку, зачем тут проверять если 100%
            // уверен что цена валидна в auction current price
            var maxBidFromAuctionParticipations = await dbContext.AuctionParticipations
                .Select(ap => (double?)ap.BidPrice) 
                .MaxAsync(cancellationToken);

            if (maxBidFromAuctionParticipations.HasValue && request.BidPrice <= maxBidFromAuctionParticipations.Value)
                return Result<AuctionParticipationMakeABigVm>.BadRequest("Введенная ставка не может быть меньше текущей цены");

            if (request.BidPrice <= await dbContext.Auctions
                .Where(a => a.Id.Equals(auctionId))
                .MaxAsync(a => a.CurrentPrice, cancellationToken)
                )
                return Result<AuctionParticipationMakeABigVm>.BadRequest("Введенная ставка не может быть меньше текущей цены");

            var auctionParticipation = new AuctionParticipation()
            {
                Id = Guid.NewGuid(),
                AuctionId = auctionId,
                UserId = currentUserService.UserId,
                BidPrice = request.BidPrice,
                BidTime = DateTime.Now
            };

            var auctionFromDb = await dbContext.Auctions.FirstOrDefaultAsync(a => a.Id.Equals(auctionId), cancellationToken)!;
            auctionFromDb!.CurrentPrice = request.BidPrice;

            await dbContext.Users.FirstOrDefaultAsync(u => u.Id == currentUserService.UserId, cancellationToken: cancellationToken);

            await dbContext.AuctionParticipations.AddAsync(auctionParticipation, cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);

            return Result<AuctionParticipationMakeABigVm>.Created(mapper.Map<AuctionParticipationMakeABigVm>(auctionParticipation));

        }
    }
}
