using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auction.Domain.Models
{
    public class AuctionParticipation
    {
        public Guid Id { get; set; }
        public Guid AuctionId { get; set; }
        public Auction Auction { get; set; }

        public Guid UserId { get; set; }
        public User User { get; set; }

        public long BidPrice { get; set; }
        public DateTime BidTime { get; set; }
    }

}
