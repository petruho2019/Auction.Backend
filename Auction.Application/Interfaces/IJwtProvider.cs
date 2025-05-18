using Auction.Application.Common.Models;
using Auction.Application.Common.Models.Dto.Tokens.CreateRefreshToken;
using Auction.Application.Common.Models.Vm.Users.Auth;
using Auction.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auction.Application.Interfaces
{
    public interface IJwtProvider
    {
        string GenerateToken(UserAuth user);
        RefreshToken GenerateRefreshToken(CreateRefreshTokenDto createRefreshToken);
    }
}
