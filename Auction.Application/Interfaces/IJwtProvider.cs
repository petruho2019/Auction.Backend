using Auction.Application.Common.Models.Dto.Tokens.CreateRefreshToken;
using Auction.Application.Common.Models.Vm.Users.Auth;
using Auction.Domain.Models;
using System.Security.Claims;

namespace Auction.Application.Interfaces
{
    public interface IJwtProvider
    {
        string GenerateAccessToken(UserAuth user);
        RefreshToken GenerateRefreshToken(CreateRefreshTokenDto createRefreshToken);
        Task<string> GenerateNewAccessTokenByRefresh(string refreshToken);
        bool IsValidAccess(string token);
        Task<bool> IsValidRefreshAsync(string token);
        IReadOnlyCollection<Claim> GetClaimsFromAccess(string token);
    }
}
