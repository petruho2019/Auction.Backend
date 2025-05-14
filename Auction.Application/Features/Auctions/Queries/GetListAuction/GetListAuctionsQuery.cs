using Auction.Application.Common.Models.Vm.Auctions.GetList;
using Auction.Domain.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auction.Application.Features.Auctions.Queries.GetListAuction
{
    public class GetListAuctionsQuery : IRequest<List<AuctionVm>>
    {

    }
}
