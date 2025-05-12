using Auction.Application.Interfaces;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auction.Application.Common.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        public CurrentUserService(HttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
        }

        private readonly HttpContextAccessor _contextAccessor;
        public string Username => _contextAccessor.HttpContext.User.Identity!.Name!;

        public Guid UserId => Guid.Parse(_contextAccessor.HttpContext.User.FindFirst("UUID")!.Value);

        
    }
}
