using Auction.Application.Interfaces;
using Auction.Domain.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Reflection.Metadata;
using System.Security.Claims;

namespace Auction.Application.Attributes.Class.Filters
{
    public class CheckAuthFilter(IConfiguration configuration, IJwtProvider jwtProvider) : IAsyncActionFilter
    {
        private readonly TimeSpan _expireAccess = DateTime.MinValue.AddMinutes(int.Parse(configuration["JwtSettings:AccessExpiresMinutes"]!)) - DateTime.Now;

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var httpContext = context.HttpContext;
            var request = context.HttpContext.Request;
            var response = context.HttpContext.Response;

            var hasAuthorizeAttr = context.ActionDescriptor.EndpointMetadata
                .OfType<AuthorizeAttribute>()
                .FirstOrDefault() != null;

            if (!hasAuthorizeAttr)
            {
                await next();
                return;
            }

            var refreshToken = request.Cookies["auction-refresh"];
            if (string.IsNullOrEmpty(refreshToken))
            {
                response.StatusCode = 401;
                context.Result = new RedirectResult("/api/auth/login");
                return;
            }

            var accessToken = request.Cookies["auction-access"];

            // Если access-токен валиден - пропускаем запрос
            if (!string.IsNullOrEmpty(accessToken) && jwtProvider.IsValidAccess(accessToken))
            {
                var identity = new ClaimsIdentity(jwtProvider.GetClaimsFromAccess(accessToken), "Custom");
                httpContext.User = new ClaimsPrincipal(identity);
                await next();
                return;
            }

            // Если access-токен невалиден, но refresh-токен валиден - обновляем access
            if (await jwtProvider.IsValidRefreshAsync(refreshToken))
            {
                var newAccess = await jwtProvider.GenerateNewAccessTokenByRefresh(refreshToken);

                if (!string.IsNullOrEmpty(newAccess))
                {
                    AppendAccessTokenToCookie(response, newAccess);
                    await next();
                    return;
                }
            }

            // Если дошли сюда - значит аутентификация не удалась
            response.StatusCode = 401;
            context.Result = new RedirectResult("/api/auth/login");
        }

        private void AppendAccessTokenToCookie(HttpResponse response, string token)
        {
            response.Cookies.Append("auction-access", token, new CookieOptions()
            {
                SameSite = SameSiteMode.Lax,
                Secure = true,
                HttpOnly = true,
                MaxAge = _expireAccess
            });
        }
    }
}
// TODO Реализовать собственный handlerbuilder.Services.AddAuthentication("Custom").AddScheme<AuthenticationSchemeOptions, CustomCookieAuthenticationHandler>("Custom", null);