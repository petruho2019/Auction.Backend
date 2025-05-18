using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auction.Domain.Models
{
    public class RefreshToken
    {
        public Guid Id { get; set; }
        public string Token { get; set; }
        public string CreatedByIp { get; set; }
        public DateTime Expires { get; set; }
        public bool IsExpire => DateTime.UtcNow >= Expires;
        public DateTime Created { get; set; }
        public DateTime? Revoked { get; set; }
        public bool IsActive => Revoked == null && !IsExpire;

        public Guid OwnerId { get; set; }
        public User Owner { get; set; }
    }
}
