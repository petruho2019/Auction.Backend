using Microsoft.EntityFrameworkCore;
using Auction.Domain.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Auction.Database.EntityTypeConfiguration
{
    class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.HasKey(p => p.Id).HasName("Product_pkey");
            builder.ToTable("Product");

            builder.Property(p => p.Name).IsRequired();
            builder.Property(p => p.Description).HasColumnType("text");
            builder.Property(p => p.Location).IsRequired();
            builder.Property(p => p.Created).HasColumnType("TIMESTAMP");


            builder
                .HasOne(p => p.User)
                .WithMany(u => u.Products)
                .HasForeignKey(p => p.UserId)
                .IsRequired();
        }

    }
}
