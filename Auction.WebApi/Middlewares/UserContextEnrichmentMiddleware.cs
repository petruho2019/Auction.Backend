using System.Diagnostics;

namespace Auction.WebApi.Middlewares
{
    public class UserContextEnrichmentMiddleware(
        RequestDelegate next,
        ILogger<UserContextEnrichmentMiddleware> logger)
    {
        public async Task InvokeAsync(HttpContext context)
        {
            var userId = context.User?.FindFirst("ID")?.Value;

            if (userId != null)
            {
                Activity.Current.SetTag("UserId", userId);
            }

            await next(context);
        }
    }
}
