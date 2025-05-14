using Auction.Application.Common.Mappings;
using Auction.Application.Common.Models;
using Auction.Application.Common.Models.Vm.Users.Auth;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auction.Application.Features.Users.Commands.Login
{
    public class LoginUserCommand : IRequest<Result<UserAuth>>
    {
        public string Username { get; set; }
        public string Password { get; set; }

    }
}
