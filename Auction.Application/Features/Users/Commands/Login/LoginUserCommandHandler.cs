using Auction.Application.Common.Models;
using Auction.Application.Common.Models.Vm.Users.Auth;
using Auction.Application.Interfaces;
using AutoMapper;
using BCrypt.Net;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auction.Application.Features.Users.Commands.Login
{
    public class LoginUserCommandHandler : BaseComponentHandler, IRequestHandler<LoginUserCommand, Result<UserAuth>>
    {
        public IJwtProvider _jwtProvider { get; set; }
        public LoginUserCommandHandler(IAuctionContext dbContext, IMapper mapper, IJwtProvider jwtProvider, ICurrentUserService currentUserService) : base(dbContext, mapper, currentUserService)
        {
            _jwtProvider = jwtProvider;
        }

        public async Task<Result<UserAuth>> Handle(LoginUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Username == request.Username, cancellationToken);

            if (user == null)
                return CreateFailureResult<UserAuth>("Пользователь с таким именем не найден");
            if (!BCrypt.Net.BCrypt.Verify(request.Password, user.Password))
                return CreateFailureResult<UserAuth>("Не верный пароль");

            var userVm = _mapper.Map<UserAuth>(user);
            userVm.Token = _jwtProvider.GenerateToken(user);

            return CreateSuccessResult(userVm);
        }
    }
}
