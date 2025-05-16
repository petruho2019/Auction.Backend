using Auction.Application.Common.Models;
using Auction.Application.Common.Models.Vm.AuctionParticipation.Create;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auction.Application.Features.AuctionParticipstions.Commands
{
    public class CreateAuctionParticipationCommand : IRequest<Result<AuctionParticipationMakeABigVm>>
    {
        public string AuctionId { get; set; }
        public double BidPrice { get; set; }
    }
}
