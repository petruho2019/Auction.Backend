using Auction.Application.Common.Models;
using Auction.Application.Common.Models.Vm.Users.Auth;
using Auction.Application.Interfaces;
using Auction.Domain.Models;
using AutoMapper;
using MediatR;

namespace Auction.Application.Features.Users.Commands.CreateUser
{
    public class CreateUserCommandHandler : BaseComponentHandler, IRequestHandler<CreateUserCommand, Result<UserAuth>>
    {
        public IJwtProvider _jwtProvider { get; set; }
        public CreateUserCommandHandler(IAuctionContext dbContext, IMapper mapper, IJwtProvider jwtProvider, ICurrentUserService currentUserService) : base(dbContext, mapper, currentUserService)
        {
            _jwtProvider = jwtProvider;
        }

        public async Task<Result<UserAuth>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            if (_dbContext.Users.Any(u => u.Username == request.Username))
                return CreateFailureResult<UserAuth>("Имя пользователя занято");
            if (_dbContext.Users.Any(u => u.Email == request.Email))
                return CreateFailureResult<UserAuth>("Пользователь с таким email уже существует");

            var user = new User()
            {
                Id = Guid.NewGuid(),
                Email = request.Email,
                Username = request.Username,
                Password = BCrypt.Net.BCrypt.HashPassword(request.Password)
            };

            await _dbContext.Users.AddAsync(user);
            await _dbContext.SaveChangesAsync(default);

            var userVm = _mapper.Map<UserAuth>(user);
            userVm.Token = _jwtProvider.GenerateToken(user);

            return CreateSuccessResult(userVm);
        }
    }
}
