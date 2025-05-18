using Auction.Application.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auction.Application.Interfaces
{
    interface IValidationTokenService
    {
        Result<string> ValidateRefreshToken(string refreshToken);
        Result<string> GenerateNewAccessByRefreshToken(string refreshToken);
    }
}
