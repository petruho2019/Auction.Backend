using Auction.Application.Common.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Auction.Application.Features.Auctions.Commands.EndAction
{
    public class CompleteAuctionCommand : IRequest<Result>
    {
        public Guid AuctionId { get; set; }
    }
}
