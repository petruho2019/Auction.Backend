using Auction.Application.Common.Models;
using Auction.Application.Common.Models.Vm;
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
    public class LoginUserCommandHandler : BaseComponentHandler, IRequestHandler<LoginUserCommand, Result<UserVm>>
    {
        public IJwtProvider _jwtProvider { get; set; }
        public LoginUserCommandHandler(IAuctionContext dbContext, IMapper mapper, IJwtProvider jwtProvider) : base(dbContext, mapper)
        {
            _jwtProvider = jwtProvider;
        }

        public async Task<Result<UserVm>> Handle(LoginUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Username == request.Username, cancellationToken);

            if (user == null)
                return CreateFailureResult<UserVm>("Пользователь с таким именем не найден");
            if (!BCrypt.Net.BCrypt.Verify(request.Password, user.Password))
                return CreateFailureResult<UserVm>("Не верный пароль");

            var userVm = _mapper.Map<UserVm>(user);
            userVm.Token = _jwtProvider.GenerateToken(user);

            return CreateSuccessResult(userVm);
        }
    }
}
