namespace Auction.Domain.Models
{
    public record AuctionParticipation
    {
        public Guid Id { get; set; }
        public Guid AuctionId { get; set; }
        public Auction Auction { get; set; }

        public Guid UserId { get; set; }
        public User User { get; set; }

        public double BidPrice { get; set; }
        public DateTime BidTime { get; set; } = DateTime.Now;
    }

}
