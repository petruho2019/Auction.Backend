using Auction.Application.Common.Models;

namespace Auction.Application.Interfaces
{
    interface IValidationTokenService
    {
        Result<string> ValidateRefreshToken(string refreshToken);
        Result<string> GenerateNewAccessByRefreshToken(string refreshToken);
    }
}
