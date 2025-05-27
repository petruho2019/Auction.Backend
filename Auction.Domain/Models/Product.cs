namespace Auction.Domain.Models
{
    public record Product
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public string Location { get; set; }
        public int Quantity { get; set; }
        public DateTime Created { get; set; } = DateTime.UtcNow;

        // TODO можно ли изменить на Owner?
        public Guid UserId { get; set; }
        public User User { get; set; }

        public List<ProductImage> Images { get; set; }

        public List<Auction> Auctions { get; set; }
    }

}
