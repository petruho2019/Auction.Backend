using Auction.Application.Common.Models;
using Auction.Application.Common.Models.Vm.Users.Auth;
using Auction.Application.Interfaces;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Auction.Application.Features.Users.Queries.Login
{
    public class LoginUserQueryHandler(IAuctionContext dbContext, IMapper mapper, ICurrentUserService currentUserService) : IRequestHandler<LoginUserQuery, Result<UserAuth>>
    {

        public async Task<Result<UserAuth>> Handle(LoginUserQuery request, CancellationToken cancellationToken)
        {
            var user = await dbContext.Users.FirstOrDefaultAsync(u => u.Username == request.Username, cancellationToken);

            if (user == null)
                return Result<UserAuth>.BadRequest("Пользователь с таким именем не найден");
            if (!BCrypt.Net.BCrypt.Verify(request.Password, user.Password))
                return Result<UserAuth>.BadRequest("Не верный пароль");

            return Result<UserAuth>.Ok(mapper.Map<UserAuth>(user));
        }
    }
}
