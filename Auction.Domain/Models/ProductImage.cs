

namespace Auction.Domain.Models
{
    public record ProductImage
    {
        public Guid Id { get; set; }
        public string Image { get; set; }

        public Guid ProductId { get; set; }
        public Product Product { get; set; }
    }

}
