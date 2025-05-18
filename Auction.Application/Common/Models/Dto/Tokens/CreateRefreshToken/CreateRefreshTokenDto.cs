using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auction.Application.Common.Models.Dto.Tokens.CreateRefreshToken
{
    public class CreateRefreshTokenDto
    {
        public string Ip { get; set; }
        public Guid UserId { get; set; }
    }
}
