using Auction.Application.Interfaces;
using Auction.Database.EntityTypeConfiguration;
using Auction.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Auction.Database
{
    public class AuctionContext : DbContext, IAuctionContext
    {
        public virtual DbSet<User> Users { get; set; } = null!;
        public virtual DbSet<Product> Products { get; set; } = null!;
        public virtual DbSet<ProductImage> ProductImages { get; set; } = null!;
        public DbSet<Domain.Models.Auction> Auctions { get; set; } = null!;
        public DbSet<AuctionParticipation> AuctionParticipations { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }

        public AuctionContext(DbContextOptions options) : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UserConfiguration("User", "Auction"));
            modelBuilder.ApplyConfiguration(new ProductConfiguration("Product", "Auction"));
            modelBuilder.ApplyConfiguration(new ProductImageConfiguration("ProductImage", "Auction"));
            modelBuilder.ApplyConfiguration(new AuctionConfiguration("Auction", "Auction"));
            modelBuilder.ApplyConfiguration(new AuctionParticipationConfiguration("AuctionParticipation", "Auction"));
            modelBuilder.ApplyConfiguration(new RefreshTokenConfiguration("RefreshToken", "Auction"));
        }
    }
}
