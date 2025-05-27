using Auction.Application.Interfaces;
using Microsoft.AspNetCore.Http;

namespace Auction.Application.Common.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _contextAccessor;
        public CurrentUserService(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
        }
        public string Username => _contextAccessor.HttpContext.User.Identity!.Name!;

        public Guid UserId => Guid.Parse(_contextAccessor.HttpContext.User.FindFirst("ID")!.Value);
    }
}
