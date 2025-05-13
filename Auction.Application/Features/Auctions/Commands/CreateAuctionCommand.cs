using Auction.Application.Common.Models;
using Auction.Application.Common.Models.Vm.Auctions;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auction.Application.Features.Auctions.Commands
{
    public class CreateAuctionCommand : IRequest<Result<CreateAuctionVm>>
    {
        public long Price { get; set; }
        public DateTime DateStart { get; set; }
        public DateTime DateEnd { get; set; }
        public int Quantity { get; set; }
        public Guid ProductId { get; set; }
    }
}
