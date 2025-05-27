using Auction.Application.Common.Models;
using Auction.Application.Interfaces;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Auction.Application.Features.Auctions.Commands.EndAction
{
    public class CompleteAuctionCommandHandler(IAuctionContext dbContext, IMapper mapper, ICurrentUserService currentUserService) : IRequestHandler<CompleteAuctionCommand, Result>
    {
        public async Task<Result> Handle(CompleteAuctionCommand request, CancellationToken cancellationToken)
        {
            // TODO возможно не нужная проверка, тк мы отпраляет этот запрос с сервера
            // перед отправкой получаем auctions из бд, так что 100% что id верно
            var auctionFromDb = await dbContext.Auctions.FirstOrDefaultAsync(a => a.Id.Equals(request.AuctionId), cancellationToken);

            if (auctionFromDb == null)
                return Result.BadRequest("Аукцион не найден");
            if (auctionFromDb.IsEnded && (auctionFromDb.End < DateTime.Now))
                return Result.BadRequest("Auction doesn't ended");

            var maxBid = dbContext.AuctionParticipations
                .Select(ap => ap.BidTime);


            // Аукцион никто не купил, не ставили ставки, покупателя нет
            if(!await maxBid.AnyAsync(cancellationToken))
            {
                auctionFromDb.IsEnded = true;

                await dbContext.SaveChangesAsync(cancellationToken);
                return Result.NoContent();
            }
                 

            var buyer = await dbContext.AuctionParticipations
                .Include(ap => ap.User)
                .Where(ap => (ap.AuctionId.Equals(request.AuctionId)) && (ap.BidTime == maxBid.Max()))
                .Select(ap => new 
                {
                    BuyerId = ap.User.Id
                }).SingleAsync(cancellationToken);

            auctionFromDb.BuyerId = buyer.BuyerId;
            auctionFromDb.IsEnded = true;

            await dbContext.SaveChangesAsync(cancellationToken);

            return Result.NoContent();
        }
    }
}
