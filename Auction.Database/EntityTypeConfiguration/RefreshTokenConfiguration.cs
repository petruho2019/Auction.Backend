using Auction.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auction.Database.EntityTypeConfiguration
{
    class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
    {
        public void Configure(EntityTypeBuilder<RefreshToken> builder)
        {
            builder.HasKey(rt => rt.Id).HasName("Refreshtoken_pkey");
            builder.ToTable("RefreshToken");

            builder.Property("Expires").HasColumnType("timestap without time zone");
            builder.Property("Created").HasColumnType("timestap without time zone");
            builder.Property("Revoked").HasColumnType("timestap without time zone");

            builder
                .HasOne(rt => rt.Owner)
                .WithMany(o => o.RefreshTokens)
                .HasForeignKey(rt => rt.OwnerId);
        }
    }
}
