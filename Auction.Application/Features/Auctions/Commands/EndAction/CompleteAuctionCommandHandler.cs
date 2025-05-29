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
            var auctionFromDb = await dbContext.Auctions.FirstOrDefaultAsync(a => a.Id.Equals(request.Auction.Id));

            if (request.Auction.IsEnded && (request.Auction.End < DateTime.Now))
                return Result.BadRequest("Auction doesn't ended");

            var maxBid = dbContext.AuctionParticipations
                .Select(ap => ap.BidTime).ToList();

            if(!maxBid.Any())
            {
                auctionFromDb!.IsEnded = true;

                await dbContext.SaveChangesAsync(cancellationToken);
                return Result.NoContent();
            }
                 
            var buyer = await dbContext.AuctionParticipations
                .Include(ap => ap.User)
                .Where(ap => (ap.AuctionId.Equals(auctionFromDb!.Id)) && (ap.BidTime == maxBid.Max()))
                .Select(ap => new 
                {
                    BuyerId = ap.User.Id
                }).SingleAsync(cancellationToken);

            auctionFromDb!.BuyerId = buyer.BuyerId;
            auctionFromDb.IsEnded = true;

            await dbContext.SaveChangesAsync(cancellationToken);

            return Result.NoContent();
        }
    }
}
