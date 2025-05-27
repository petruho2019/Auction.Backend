using Auction.Application.Common.Models;
using Auction.Application.Common.Models.Vm.AuctionParticipation.Create;
using Auction.Application.Features.AuctionParticipstions.Commands;
using Auction.Application.Features.Auctions.Commands.CreateAuction;
using Auction.Application.Interfaces;
using Auction.Domain.Models;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auction.Application.Hubs.Auction
{
    public class AuctionHub : BaseHub
    {
        public AuctionHub(IMediator mediator) : base(mediator)
        {
        }

        public override Task OnConnectedAsync()
        {
            Console.WriteLine("OnConnected, connection id: " + Context.ConnectionId);
            return base.OnConnectedAsync();
        }

        public async Task<bool> MakeABid(int bidPrice, string auctionId)
        {
            var result = await _mediator.Send(new CreateAuctionParticipationCommand()
            {
                AuctionId = auctionId,
                BidPrice = bidPrice
            });

            if (!result.IsSuccess)
                throw new HubException(result.Error.ErrorMessage);

            await Clients
                .Group(auctionId)
                .SendAsync("SuccessMakeABid", result.Success.Data!);

            return true;
            
        }
        public async Task JoinAuction(string auctionId)
        {
            Console.WriteLine("Присоединился к аукциону " + auctionId);
            await Groups.AddToGroupAsync(Context.ConnectionId, auctionId.ToString());
        }
    }
}
