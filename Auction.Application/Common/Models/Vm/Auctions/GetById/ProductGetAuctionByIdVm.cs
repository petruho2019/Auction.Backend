using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auction.Application.Common.Models.Vm.Auctions.GetById
{
    public class ProductGetAuctionByIdVm
    {
        public string Name { get; set; }
        public string? Description { get; set; }
        public string Location { get; set; }
        public List<string> Images { get; set; }
    }
}
