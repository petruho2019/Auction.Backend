using Auction.Application.Common.Models.Vm.Users.Auth;
using MediatR;

namespace Auction.Application.Features.Users.Queries.GetCurrentUserInfo
{
    public record GetUserInfoQuery : IRequest<UserInfoVm>;
}
