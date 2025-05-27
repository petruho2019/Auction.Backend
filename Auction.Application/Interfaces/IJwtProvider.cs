using Auction.Application.Common.Models.Dto.Tokens.CreateRefreshToken;
using Auction.Application.Common.Models.Vm.Users.Auth;
using Auction.Domain.Models;

namespace Auction.Application.Interfaces
{
    public interface IJwtProvider
    {
        string GenerateToken(UserAuth user);
        RefreshToken GenerateRefreshToken(CreateRefreshTokenDto createRefreshToken);
        bool ValidateAccess(string token);
        bool ValidateRefreshWithCache(string token, string userId);
    }
}
