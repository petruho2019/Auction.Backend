using Auction.Application.Common.Models.Vm.Users.Auth;
using Auction.Application.Features.Users.Queries.GetCurrentUserInfo;
using Auction.Application.Interfaces;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Auction.Application.Features.Users.Queries.GetUserInfoFromToken
{
    class GetUserInfoQueryHandler(IAuctionContext dbContext, IMapper mapper, ICurrentUserService currentUserService) : IRequestHandler<GetUserInfoQuery, UserInfoVm>
    {
        public async Task<UserInfoVm> Handle(GetUserInfoQuery request, CancellationToken cancellationToken)
        {
            var user = await dbContext.Users.FirstOrDefaultAsync(u => u.Id.Equals(currentUserService.UserId)!);

            return new() 
            { 
                Email = user!.Email, 
                Username = user.Username 
            };
        }
    }
}
