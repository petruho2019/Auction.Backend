using Auction.Application.Common.Models;
using Auction.Application.Common.Models.Vm.Users.Auth;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auction.Application.Features.Users.Commands.CreateUser
{
    public class CreateUserCommand : IRequest<Result<UserAuth>>
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
    }
}
