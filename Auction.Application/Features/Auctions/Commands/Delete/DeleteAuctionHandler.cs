using Auction.Application.Common.Models;
using Auction.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Auction.Application.Features.Auctions.Commands.Delete
{
    public class DeleteAuctionHandler(IAuctionContext context) : IRequestHandler<DeleteAuctionCommand, Result>
    {
        public async Task<Result> Handle(DeleteAuctionCommand request, CancellationToken cancellationToken)
        {
            var auctionFromDb = await context.Auctions.FirstOrDefaultAsync(a => a.Id.Equals(request.AuctionId));
            if (auctionFromDb == null)
                return Result.BadRequest("Аукцион не существует");

            context.Auctions.Remove(auctionFromDb);
            await context.SaveChangesAsync(cancellationToken);

            return Result.Ok();
        }
    }
}
