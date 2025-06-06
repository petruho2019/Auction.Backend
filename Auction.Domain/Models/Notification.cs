namespace Auction.Domain.Models
{
    public record Notification
    {
        public Guid Id { get; set; }
        public string Message { get; set; }

        public Guid UserId { get; set; }
        public User User { get; set; }
        public Guid AuctionId { get; set; }
        public Auction Auction { get; set; }
    }
}
