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
    public class AuctionParticipationConfiguration : IEntityTypeConfiguration<AuctionParticipation>
    {
        public void Configure(EntityTypeBuilder<AuctionParticipation> builder)
        {
            builder.HasKey(ap => ap.Id).HasName("AuctionParticipation_pkey");
            builder.ToTable("AuctionParticipation");

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
