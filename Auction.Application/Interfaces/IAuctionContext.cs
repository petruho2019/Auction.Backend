using Auction.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Auction.Application.Interfaces
{
    public interface IAuctionContext
    {
        DbSet<User> Users { get; set; }
        DbSet<Product> Products { get; set; }
        DbSet<ProductImage> ProductImages { get; set; }
        DbSet<Domain.Models.Auction> Auctions { get; set; }
        DbSet<AuctionParticipation> AuctionParticipations { get; set; }
        DbSet<RefreshToken> RefreshTokens { get; set; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
