using Auction.Application.Common.Models;
using Auction.Application.Common.Models.Vm.Auctions.GetById;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auction.Application.Features.Auctions.Queries.GetById
{
    public class GetAuctionByIdQuery : IRequest<Result<GetAuctionByIdVm>>
    {
        public string AuctionId { get; set; }
    }
}
