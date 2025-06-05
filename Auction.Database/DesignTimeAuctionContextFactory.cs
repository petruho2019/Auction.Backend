using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Auction.Database;

public class DesignTimeAuctionContextFactory : IDesignTimeDbContextFactory<AuctionContext>
{
    public AuctionContext CreateDbContext(string[] args)
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory())))
            .AddJsonFile("appsettings.Development.json", optional: false)
            .Build();

        var optionsBuilder = new DbContextOptionsBuilder<AuctionContext>();
        var connectionString = configuration["ConnectionString"];

        optionsBuilder.UseNpgsql(connectionString);

        return new AuctionContext(optionsBuilder.Options);
    }
}
