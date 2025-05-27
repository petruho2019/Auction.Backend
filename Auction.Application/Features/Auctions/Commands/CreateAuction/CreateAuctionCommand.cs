using Auction.Application.Common.Models;
using Auction.Application.Common.Models.Vm.Auctions.Create;
using MediatR;

namespace Auction.Application.Features.Auctions.Commands.CreateAuction
{
    public record CreateAuctionCommand : IRequest<Result<CreateAuctionVm>>
    {
        public double Price { get; set; }
        public DateTime DateStart { get; set; }
        public DateTime DateEnd { get; set; }
        public int Quantity { get; set; }
        public Guid ProductId { get; set; }
    }
}
