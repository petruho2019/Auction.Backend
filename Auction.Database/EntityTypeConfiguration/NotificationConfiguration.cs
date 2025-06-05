using Auction.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Auction.Database.EntityTypeConfiguration
{
    class NotificationConfiguration(string tableName, string schema) : IEntityTypeConfiguration<Notification>
    {
        public void Configure(EntityTypeBuilder<Notification> builder)
        {
            builder.HasKey(a => a.Id).HasName("Notification_pkey");
            builder.ToTable(tableName, schema);

            builder
                .HasOne(a => a.User)
                .WithMany(u => u.Notifications)
                .HasForeignKey(a => a.UserId);
        }
    }
}
