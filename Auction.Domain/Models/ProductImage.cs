using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auction.Domain.Models
{
    public class ProductImage
    {
        public Guid Id { get; set; }
        public byte[] Image { get; set; }
        public Guid ProductId { get; set; }
        public Product Product { get; set; }
    }

}
