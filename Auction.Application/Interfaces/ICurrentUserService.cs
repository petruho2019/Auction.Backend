using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auction.Application.Interfaces
{
    public interface ICurrentUserService
    {
        string Username { get; }
        Guid UserId { get; }
    }
}
