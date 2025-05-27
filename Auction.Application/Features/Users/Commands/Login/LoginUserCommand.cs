using Auction.Application.Common.Models;
using Auction.Application.Common.Models.Vm.Users.Auth;
using MediatR;

namespace Auction.Application.Features.Users.Commands.Login
{
    public record LoginUserCommand : IRequest<Result<UserAuth>>
    {
        public string Username { get; set; }
        public string Password { get; set; }

    }
}
