using Auction.Application.Common.Models;
using Auction.Application.Common.Models.Vm;
using Auction.Application.Interfaces;
using Auction.Domain.Models;
using AutoMapper;
using MediatR;

namespace Auction.Application.Features.Users.Commands.CreateUser
{
    public class CreateUserCommandHandler : BaseComponentHandler, IRequestHandler<CreateUserCommand, Result<UserVm>>
    {
        public IJwtProvider _jwtProvider { get; set; }
        public CreateUserCommandHandler(IAuctionContext dbContext, IMapper mapper, IJwtProvider jwtProvider) : base(dbContext, mapper)
        {
            _jwtProvider = jwtProvider;
        }

        public async Task<Result<UserVm>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            if (_dbContext.Users.Any(u => u.Username == request.Username))
                return CreateFailureResult<UserVm>("Пользователь с таким именем пользователя занят");
            if (_dbContext.Users.Any(u => u.Email == request.Email))
                return CreateFailureResult<UserVm>("Пользователь с таким email уже существует");

            var user = new User()
            {
                Id = Guid.NewGuid(),
                Email = request.Email,
                Username = request.Username,
                Password = BCrypt.Net.BCrypt.HashPassword(request.Password)
            };

            await _dbContext.Users.AddAsync(user);
            await _dbContext.SaveChangesAsync(default);

            var userVm = _mapper.Map<UserVm>(user);
            userVm.Token = _jwtProvider.GenerateToken(user);

            return CreateSuccessResult(userVm);
        }
    }
}
