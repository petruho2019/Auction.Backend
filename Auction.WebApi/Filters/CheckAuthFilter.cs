using Auction.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Reflection.Metadata;

namespace Auction.Application.Attributes.Class.Filters
{
    public class CheckAuthFilter(IJwtProvider jwtProvider) : IAsyncActionFilter
    {

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var httpContext = context.HttpContext;
            var request = context.HttpContext.Request;
            var response = context.HttpContext.Response;

            var HasAuthorizeAttr = context.ActionDescriptor.EndpointMetadata
             .OfType<AuthorizeAttribute>()
             .FirstOrDefault() != null;

            if (HasAuthorizeAttr)
            {
                var refreshToken = request.Cookies["auction-refresh"];
                if (refreshToken == null)
                {
                    response.StatusCode = 401;
                    context.Result = new ObjectResult("refresh token не найден") { StatusCode = 401 };
                    return;
                }

                if (jwtProvider.ValidateRefreshWithCache(refreshToken, httpContext.User.FindFirst("ID")!.Value))
                {

                }

                var accessToken = request.Cookies["auction-access"];
                if (accessToken == null) {
                    response.StatusCode = 401;
                    context.Result = new ObjectResult("Access token не найден") { StatusCode = 401 };
                    return;
                }

                if (!jwtProvider.ValidateAccess(accessToken))
                {
                    Console.WriteLine("Невалидный токен access");
                }


            } // TODO Доделать фильтр

            await next();
            Console.WriteLine("After invoke next()");
        }
    }
}
