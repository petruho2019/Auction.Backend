namespace Auction.Database
{
    public class DbInitializer
    {
        public static void Initialize(AuctionContext context)
            => context.Database.EnsureCreated();
    }
}
