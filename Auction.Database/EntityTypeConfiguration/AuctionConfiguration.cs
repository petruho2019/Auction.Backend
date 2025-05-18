using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auction.Database.EntityTypeConfiguration
{
    class AuctionConfiguration : IEntityTypeConfiguration<Domain.Models.Auction>
    {
        public void Configure(EntityTypeBuilder<Domain.Models.Auction> builder)
        {
            builder.HasKey(a => a.Id).HasName("Auction_pkey");
            builder.ToTable("Auction");
            builder.Property(a => a.Start).HasColumnType("timestamp without time zone");
            builder.Property(a => a.End).HasColumnType("timestamp without time zone");

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
