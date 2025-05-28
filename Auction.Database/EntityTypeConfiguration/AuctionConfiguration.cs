using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Auction.Database.EntityTypeConfiguration
{
    class AuctionConfiguration(string tableName, string schema) : IEntityTypeConfiguration<Domain.Models.Auction>
    {
        public void Configure(EntityTypeBuilder<Domain.Models.Auction> builder)
        {
            builder.HasKey(a => a.Id).HasName("Auction_pkey");
            builder.ToTable(tableName, schema);
            builder.Property(a => a.Start).HasColumnType("TIMESTAMP");
            builder.Property(a => a.End).HasColumnType("TIMESTAMP");

            builder
                .HasOne(a => a.Buyer)
                .WithMany()
                .HasForeignKey(a => a.BuyerId);

            builder
                .HasOne(a => a.Product)
                .WithMany(p => p.Auctions)
                .HasForeignKey(a => a.ProductId);

            builder
                .HasOne(a => a.Creator)
                .WithMany(u => u.CretedAuctions)
                .HasForeignKey(a => a.CreatorId);
        }
    }
}
