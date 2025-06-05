using Auction.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Auction.Database.EntityTypeConfiguration
{
    public class AuctionParticipationConfiguration(string tableName, string schema) : IEntityTypeConfiguration<AuctionParticipation>
    {
        public void Configure(EntityTypeBuilder<AuctionParticipation> builder)
        {
            builder.HasKey(ap => ap.Id).HasName("AuctionParticipation_pkey");
            builder.ToTable(tableName, schema);

            builder.Property(p => p.BidTime).HasColumnType("TIMESTAMP");

            builder
                .HasOne(ap => ap.Auction)
                .WithMany(a => a.Participations)
                .HasForeignKey(ap => ap.AuctionId);

            builder
                .HasOne(ap => ap.User)
                .WithMany(u => u.Participations)
                .HasForeignKey(ap => ap.UserId);

        }
    }
}
