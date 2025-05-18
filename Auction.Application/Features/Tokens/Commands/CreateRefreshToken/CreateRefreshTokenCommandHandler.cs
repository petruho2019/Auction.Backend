using Auction.Application.Common.Models;
using Auction.Application.Common.Models.Dto.Tokens.CreateRefreshToken;
using Auction.Application.Common.Models.Vm.Users.Auth;
using Auction.Application.Interfaces;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auction.Application.Features.Tokens.Commands.CreateRefreshToken
{
    class CreateRefreshTokenCommandHandler : BaseComponentHandler, IRequestHandler<CreateRefreshTokenCommand, Result<string>>
    {
        private readonly IJwtProvider _jwtProvider;
        private readonly IConfiguration _configuration;
        public CreateRefreshTokenCommandHandler(IConfiguration configuration, IJwtProvider jwtProvider, IAuctionContext dbContext, IMapper mapper, ICurrentUserService currentUserService) : base(dbContext, mapper, currentUserService)
        {
            _jwtProvider = jwtProvider;
            _configuration = configuration;
        }

        public async Task<Result<string>> Handle(CreateRefreshTokenCommand request, CancellationToken cancellationToken)
        {
            if (!request.SkipDeviceLimitCheck && _dbContext.RefreshTokens.Count(rt => rt.OwnerId == request.UserId) > int.Parse(_configuration["Auth:MaxDevicesPerUser"]!))
                return CreateFailureResult<string>("Вы не можете иметь больше 5 авторизированных устройств");

            var refreshToken = _jwtProvider.GenerateRefreshToken(_mapper.Map<CreateRefreshTokenDto>(request));

            await _dbContext.RefreshTokens.AddAsync(refreshToken, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return CreateSuccessResult(refreshToken.Token);
        }
    }
}
