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
    public class JwtProvider(IConfiguration configuration) : IJwtProvider
    {
        public RefreshToken GenerateRefreshToken(CreateRefreshTokenDto createRefreshToken)
        {
            var refreshToken = new RefreshToken()
            {
                Id = Guid.NewGuid(),
                Token = RandomString(25) + Guid.NewGuid(),
                CreatedByIp = createRefreshToken.Ip,
                Expires = DateTime.UtcNow.AddMonths(int.Parse(configuration["JwtSettings:RefreshExpiresMonth"]!)),
                Created = DateTime.UtcNow,
                OwnerId = createRefreshToken.UserId
            };

            return refreshToken;
        }

        public string GenerateToken(UserAuth user)
        {
            var signingCredentials = new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JwtSettings:SecretKey"]!)), SecurityAlgorithms.HmacSha256);

            Claim[] claims = [
                new("Username", user.Username),
                new("ID", user.UserId.ToString()),
                ];

            var token = new JwtSecurityToken(
               signingCredentials: signingCredentials,
               expires: DateTime.UtcNow.AddHours(int.Parse(configuration["JwtSettings:AccessExpiresHours"]!)),
               claims: claims
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public bool ValidateAccess(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var expTime = DateTime.UnixEpoch.AddSeconds(long.Parse(handler.ReadJwtToken(token).Claims.FirstOrDefault(c => c.Type.Equals("exp"))!.Value));
            
            return expTime < DateTime.UtcNow;
        }

        public bool ValidateRefreshWithCache(string token, string userId)
        {
            throw new NotImplementedException();
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