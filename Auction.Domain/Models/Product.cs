using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auction.Domain.Models
{
    public class Product
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public string Location { get; set; }
        public int Quantity { get; set; }
        public long Price { get; set; }
        public DateTime DateCreate { get; set; } = DateTime.UtcNow;
        public Guid UserId { get; set; }
        public User User { get; set; }
        public List<ProductImage> Images { get; set; }
    }

}
