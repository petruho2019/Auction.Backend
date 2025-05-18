using Auction.Application.Interfaces;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auction.Application.Hubs
{
    public class BaseHub : Hub
    {
        public readonly IMediator _mediator;

        public BaseHub(
            IMediator mediator
            )
        {
            _mediator = mediator;
        }
    }
}
