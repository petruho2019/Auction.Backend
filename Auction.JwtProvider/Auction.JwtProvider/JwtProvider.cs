using Auction.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Auction.Application.Interfaces;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using Auction.Application.Common.Models.Vm.Users.Auth;
using Auction.Application.Common.Models;
using Auction.Application.Common.Models.Dto.Tokens.CreateRefreshToken;

namespace Auction.JwtProvider
{
    public class JwtProvider : IJwtProvider
    {
        private readonly IConfiguration _configuration;
        public JwtProvider(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public RefreshToken GenerateRefreshToken(CreateRefreshTokenDto createRefreshToken)
        { 

            var refreshToken = new RefreshToken()
            {
                Id = Guid.NewGuid(),
                Token = RandomString(25) + Guid.NewGuid(),
                CreatedByIp = createRefreshToken.Ip,
                Expires = DateTime.UtcNow.AddMonths(int.Parse(_configuration["JwtSettings:RefreshExpiresMonth"]!)),
                Created = DateTime.UtcNow,
                OwnerId = createRefreshToken.UserId
            };

            return refreshToken;
        }

        public string GenerateToken(UserAuth user)
        {
            var signingCredentials = new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:SecretKey"]!)), SecurityAlgorithms.HmacSha256);

            Claim[] claims = [
                new("Username", user.Username),
                new("ID", user.UserId.ToString()),
                ];

            var token = new JwtSecurityToken(
               signingCredentials: signingCredentials,
               expires: DateTime.UtcNow.AddHours(int.Parse(_configuration["JwtSettings:AccessExpiresHours"]!)),
               claims: claims
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
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