using Auction.Application.Common.Models.Vm.Users.Auth;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auction.Application.Features.Users.Queries.GetCurrentUserInfo
{
    public class GetUserInfoQuery : IRequest<UserInfoVm>
    {
    }
}
