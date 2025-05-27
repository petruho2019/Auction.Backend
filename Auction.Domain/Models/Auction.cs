using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auction.Domain.Models
{
    public record Auction
    {
        public Guid Id { get; set; }
        public double CurrentPrice { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public int Quantity { get; set; }
        public bool IsEnded { get; set; }

        public Guid? BuyerId { get; set; }
        public User? Buyer { get; set; }

        public Guid ProductId { get; set; }
        public Product Product { get; set; }

        public Guid CreatorId { get; set; }
        public User Creator { get; set; }

        public List<AuctionParticipation> Participations { get; set; }
    }

}
