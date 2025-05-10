using Auction.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auction.Database
{
    public class DbInitializer
    {
        public static void Initialize(AuctionContext context) 
            => context.Database.EnsureCreated();
        
    }
}
