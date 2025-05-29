namespace Auction.Application.Common.Models.Vm.Auctions.Create
{
    public class ProductCreateAuctionVm
    {
        public string Name { get; set; }
        public string? Description { get; set; }
        public string Location { get; set; }
        public List<string> Images { get; set; }
    }
}
