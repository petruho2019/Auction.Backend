using MediatR;
using Microsoft.AspNetCore.SignalR;

namespace Auction.Application.Hubs
{
    public class BaseHub : Hub
    {
        public readonly IMediator mediator;

        public BaseHub(
            IMediator mediator
            )
        {
            this.mediator = mediator;
        }
    }
}
