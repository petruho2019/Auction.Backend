using Auction.Domain.Models;
using System.Text;
using Microsoft.Extensions.Configuration;
using Auction.Application.Interfaces;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using Auction.Application.Common.Models.Vm.Users.Auth;
using Auction.Application.Common.Models.Dto.Tokens.CreateRefreshToken;

namespace Auction.JwtProvider
{
    public class JwtProvider(ICacheService cacheService, IConfiguration configuration, ICurrentUserService currentUserService) : IJwtProvider
    {
        public RefreshToken GenerateRefreshToken(CreateRefreshTokenDto createRefreshToken)
        {
            var refreshToken = new RefreshToken()
            {
                Id = Guid.NewGuid(),
                Token = RandomString(25) + Guid.NewGuid(),
                CreatedByIp = createRefreshToken.Ip,
                Expires = DateTime.Now.AddMonths(int.Parse(configuration["JwtSettings:RefreshExpiresMonths"]!)),
                Created = DateTime.Now,
                OwnerId = createRefreshToken.UserId
            };

            return refreshToken;
        }

        public string GenerateAccessToken(UserAuth user)
        {
            var signingCredentials = new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JwtSettings:SecretKey"]!)), SecurityAlgorithms.HmacSha256);

            Claim[] claims = [
                new("Username", user.Username),
                new("ID", user.UserId.ToString()),
                ];

            var token = new JwtSecurityToken(
               signingCredentials: signingCredentials,
               expires: DateTime.Now.AddMinutes(int.Parse(configuration["JwtSettings:AccessExpiresMinutes"]!)),
               claims: claims
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public bool IsValidAccess(string token)
        {
            if (string.IsNullOrWhiteSpace(token))
            {
                return false;
            }

            try
            {
                if (!new JwtSecurityTokenHandler().CanReadToken(token))
                {
                    return false;
                }

                var jwtToken = new JwtSecurityTokenHandler().ReadJwtToken(token);

                var expClaim = jwtToken.Claims.FirstOrDefault(c => c.Type.Equals("exp", StringComparison.Ordinal));
                if (expClaim == null || string.IsNullOrWhiteSpace(expClaim.Value))
                {
                    return false;
                }

                if (!long.TryParse(expClaim.Value, out var expUnixTime))
                {
                    return false;
                }

                var expTime = DateTime.UnixEpoch.AddSeconds(expUnixTime);

                return expTime > DateTime.Now;
            }
            catch (Exception ex) when (ex is ArgumentException || ex is SecurityTokenException)
            {
                return false;
            }
        }

        public async Task<bool> IsValidRefreshAsync(string token)
        {
            if (string.IsNullOrWhiteSpace(token))
            {
                return false;
            }

            var refreshFromCache = await cacheService.GetAsync<CachedRefreshTokenDto>($"refresh:{currentUserService.UserId}");

            if (refreshFromCache == null)
                return false;
            if (!refreshFromCache.Token.Equals(token))
                return false;

            return refreshFromCache.IsActive ? true : false;
        }

        public async Task<string> GenerateNewAccessTokenByRefresh(string token)
        {
            var refreshFromCache = await cacheService.GetAsync<CachedRefreshTokenDto>($"refresh:{currentUserService.UserId}");

            if (refreshFromCache == null)
                return String.Empty;
            if (!refreshFromCache.Token.Equals(token))
                return String.Empty;

            return GenerateAccessToken(new()
            {
                UserId = refreshFromCache.OwnerId,
                Username = refreshFromCache.OwnerUsername
            });
        }

        public IReadOnlyCollection<Claim> GetClaimsFromAccess(string token)
        {
            var jwtToken = new JwtSecurityTokenHandler().ReadJwtToken(token);

            return [.. jwtToken.Claims];
        }

        private string RandomString(int length)
        {
            var random = new Random();
            var chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
            .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        
    }
}