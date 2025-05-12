using Auction.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auction.Application.Interfaces
{
    public interface IAuctionContext
    {
        DbSet<User> Users { get; set; }
        DbSet<Product> Products { get; set; }
        DbSet<ProductImage> ProductImages { get; set; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
