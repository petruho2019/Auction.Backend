using Auction.Application.Common.Models.Vm.Users.Auth;
using Auction.Application.Features.Users.Queries.GetCurrentUserInfo;
using Auction.Application.Interfaces;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auction.Application.Features.Users.Queries.GetUserInfoFromToken
{
    class GetUserInfoQueryHandler : BaseComponentHandler, IRequestHandler<GetUserInfoQuery, UserInfoVm>
    {
        public GetUserInfoQueryHandler(IAuctionContext dbContext, IMapper mapper, ICurrentUserService currentUserService) : base(dbContext, mapper, currentUserService)
        {
        }

        public async Task<UserInfoVm> Handle(GetUserInfoQuery request, CancellationToken cancellationToken)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id.Equals(_userCurrentService.UserId)!);

            return new() 
            { 
                Email = user!.Email, 
                Username = user.Username 
            };
        }
    }
}
