using Auction.Application.Common.Models;
using Auction.Application.Common.Models.Dto.Tokens.CreateRefreshToken;
using Auction.Application.Interfaces;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Configuration;

namespace Auction.Application.Features.Tokens.Commands.CreateRefreshToken
{
    class CreateRefreshTokenCommandHandler(IConfiguration configuration, IJwtProvider jwtProvider, IAuctionContext dbContext, IMapper mapper, ICurrentUserService currentUserService) : IRequestHandler<CreateRefreshTokenCommand, Result<string>>
    {

        public async Task<Result<string>> Handle(CreateRefreshTokenCommand request, CancellationToken cancellationToken)
        {
            if (!request.SkipDeviceLimitCheck && dbContext.RefreshTokens.Count(rt => rt.OwnerId == request.UserId) > int.Parse(configuration["Auth:MaxDevicesPerUser"]!))
                return Result<string>.BadRequest("Вы не можете иметь больше 5 авторизированных устройств");

            var refreshToken = jwtProvider.GenerateRefreshToken(mapper.Map<CreateRefreshTokenDto>(request));

            await dbContext.RefreshTokens.AddAsync(refreshToken, cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);

            return Result<string>.Ok(refreshToken.Token);
        }
    }
}
