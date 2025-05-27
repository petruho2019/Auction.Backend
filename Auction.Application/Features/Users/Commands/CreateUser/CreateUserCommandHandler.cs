using Auction.Application.Common.Models;
using Auction.Application.Common.Models.Vm.Users.Auth;
using Auction.Application.Interfaces;
using Auction.Domain.Models;
using AutoMapper;
using MediatR;

namespace Auction.Application.Features.Users.Commands.CreateUser
{
    public class CreateUserCommandHandler(IAuctionContext dbContext, IMapper mapper, ICurrentUserService currentUserService) : IRequestHandler<CreateUserCommand, Result<UserAuth>>
    {
        public async Task<Result<UserAuth>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            if (dbContext.Users.Any(u => u.Username == request.Username))
                return Result<UserAuth>.BadRequest("Имя пользователя занято");
            if (dbContext.Users.Any(u => u.Email == request.Email))
                return Result<UserAuth>.BadRequest("Пользователь с таким email уже существует");

            var user = new User()
            {
                Id = Guid.NewGuid(),
                Email = request.Email,
                Username = request.Username,
                Password = BCrypt.Net.BCrypt.HashPassword(request.Password)
            };

            await dbContext.Users.AddAsync(user);
            await dbContext.SaveChangesAsync(default);

            return Result<UserAuth>.Created(mapper.Map<UserAuth>(user));
        }
    }
}
