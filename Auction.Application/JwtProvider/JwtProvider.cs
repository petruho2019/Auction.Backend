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

namespace Auction.Application.JwtProvider
{
    public class JwtProvider : IJwtProvider
    {
        private readonly IConfiguration _configuration;
        public JwtProvider(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public string GenerateToken(User user)
        {
            var signingCredentials = new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:SecretKey"]!)), SecurityAlgorithms.HmacSha256);

            Claim[] claims = [
                new("Username", user.Username),
                new("UUID", user.Id.ToString()),
                ];

            var token = new JwtSecurityToken(
               signingCredentials: signingCredentials,
               expires: DateTime.UtcNow.AddMonths(int.Parse(_configuration["JwtSettings:ExpiresHours"]!)),
               claims: claims
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
