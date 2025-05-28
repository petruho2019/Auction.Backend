using Auction.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Auction.Database.EntityTypeConfiguration
{
    public class ProductImageConfiguration(string tableName, string schema) : IEntityTypeConfiguration<ProductImage>
    {
        public void Configure(EntityTypeBuilder<ProductImage> builder)
        {
            builder.HasKey(pi => pi.Id).HasName("ProductImage_pkey");
            builder.ToTable(tableName, schema);

            builder.Property(pi => pi.Image).IsRequired();

            builder
                .HasOne(pi => pi.Product)
                .WithMany(p => p.Images)
                .HasForeignKey(pi => pi.ProductId);
        }
    }

}
