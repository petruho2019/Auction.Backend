using Auction.Application.Common.Models;
using Auction.Application.Common.Models.Vm.Auctions;
using Auction.Application.Interfaces;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auction.Application.Features.Auctions.Commands.EndAction
{
    public class CompleteAuctionCommandHandler : BaseComponentHandler, IRequestHandler<CompleteAuctionCommand, Result>
    {
        public CompleteAuctionCommandHandler(IAuctionContext dbContext, IMapper mapper, ICurrentUserService currentUserService) : base(dbContext, mapper, currentUserService)
        {
        }

        public async Task<Result> Handle(CompleteAuctionCommand request, CancellationToken cancellationToken)
        {
            // TODO возможно не нужная проверка, тк мы отпраляет этот запрос с сервера
            // перед отправкой получаем auctions из бд, так что 100% что id верно
            var auctionFromDb = await _dbContext.Auctions.FirstOrDefaultAsync(a => a.Id.Equals(request.AuctionId), cancellationToken);

            if (auctionFromDb == null)
                return CreateFailureResult("Аукцион не найден");
            if (auctionFromDb.IsEnded && (auctionFromDb.DateEnd < DateTime.Now))
                return CreateFailureResult("Auction doesn't ended");

            var maxBid = _dbContext.AuctionParticipations
                .Select(ap => ap.BidTime);


            // Аукцион никто не купил, не ставили ставки, покупателя нет
            if(!await maxBid.AnyAsync(cancellationToken))
            {
                auctionFromDb.IsEnded = true;

                await _dbContext.SaveChangesAsync(cancellationToken);
                return CreateSuccessResult();
            }
                 

            var buyer = await _dbContext.AuctionParticipations
                .Include(ap => ap.User)
                .Where(ap => (ap.AuctionId.Equals(request.AuctionId)) && (ap.BidTime == maxBid.Max()))
                .Select(ap => new 
                {
                    BuyerId = ap.User.Id
                }).SingleAsync(cancellationToken);

            auctionFromDb.BuyerId = buyer.BuyerId;
            auctionFromDb.IsEnded = true;

            await _dbContext.SaveChangesAsync(cancellationToken);

            return CreateSuccessResult();
        }
    }
}
