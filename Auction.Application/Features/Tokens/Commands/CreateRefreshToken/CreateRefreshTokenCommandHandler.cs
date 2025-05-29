using Auction.Application.Common.Models;
using Auction.Application.Common.Models.Dto.Tokens.CreateRefreshToken;
using Auction.Application.Interfaces;
using Auction.Domain.Models;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Configuration;

namespace Auction.Application.Features.Tokens.Commands.CreateRefreshToken
{
    class CreateRefreshTokenCommandHandler(ICacheService cacheService, IConfiguration configuration, IJwtProvider jwtProvider, IAuctionContext dbContext, IMapper mapper) : IRequestHandler<CreateRefreshTokenCommand, Result<string>>
    {

        public async Task<Result<string>> Handle(CreateRefreshTokenCommand request, CancellationToken cancellationToken)
        {
            var refreshTokenFromCache = await cacheService.GetAsync<RefreshToken>($"refresh:{request.UserId}", cancellationToken);
            if (refreshTokenFromCache != null)
                return Result<string>.Ok(refreshTokenFromCache.Token);

            if (!request.SkipDeviceLimitCheck && dbContext.RefreshTokens.Count(rt => rt.OwnerId == request.UserId) > int.Parse(configuration["Auth:MaxDevicesPerUser"]!))
                return Result<string>.BadRequest($"Вы не можете иметь больше {int.Parse(configuration["Auth:MaxDevicesPerUser"]!)} авторизированных устройств");

            var refreshToken = jwtProvider.GenerateRefreshToken(mapper.Map<CreateRefreshTokenDto>(request));

            var expire = refreshToken.Expires - DateTime.Now;

            var cachedRefreshToken = mapper.Map<CachedRefreshTokenDto>(refreshToken);
            cachedRefreshToken.OwnerUsername = request.Username;
            cachedRefreshToken.OwnerEmail = request.Email;

            await cacheService.SetAsync($"refresh:{refreshToken.Token}", cachedRefreshToken, expire, cancellationToken);

            await dbContext.RefreshTokens.AddAsync(refreshToken, cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);

            return Result<string>.Created(refreshToken.Token);
        }
    }
}
