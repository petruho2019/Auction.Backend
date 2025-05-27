namespace Auction.Domain.Models
{
    public record User
    {
        public Guid Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }

        public List<Product> Products { get; set; }

        public List<Auction> CretedAuctions { get; set; }

        public List<AuctionParticipation> Participations { get; set; }

        public List<RefreshToken> RefreshTokens { get; set; }
    }
}
