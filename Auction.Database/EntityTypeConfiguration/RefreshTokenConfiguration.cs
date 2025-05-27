using Auction.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Auction.Database.EntityTypeConfiguration
{
    class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
    {
        public void Configure(EntityTypeBuilder<RefreshToken> builder)
        {
            builder.HasKey(rt => rt.Id).HasName("Refreshtoken_pkey");
            builder.ToTable("RefreshToken");

            builder.Property("Expires").HasColumnType("TIMESTAMP");
            builder.Property("Created").HasColumnType("TIMESTAMP");
            builder.Property("Revoked").HasColumnType("TIMESTAMP");

            builder
                .HasOne(rt => rt.Owner)
                .WithMany(o => o.RefreshTokens)
                .HasForeignKey(rt => rt.OwnerId);
        }
    }
}
