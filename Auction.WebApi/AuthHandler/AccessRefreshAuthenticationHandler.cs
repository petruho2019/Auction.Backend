using Auction.Application.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text.Encodings.Web;

namespace Auction.WebApi.AuthHandler
{
    public class AccessRefreshAuthenticationHandler(IConfiguration configuration, IJwtProvider jwtProvider, IOptionsMonitor<AuthenticationSchemeOptions> options, ILoggerFactory logger, UrlEncoder encoder, TimeProvider clock) : AuthenticationHandler<AuthenticationSchemeOptions>(options, logger, encoder)
    {
        private readonly TimeSpan _expireAccess = DateTime.Now.AddMinutes(int.Parse(configuration["JwtSettings:AccessExpiresMinutes"]!)) - DateTime.Now;
        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            var refreshToken = Request.Cookies["auction-refresh"];
            if (string.IsNullOrEmpty(refreshToken))
            {
                Response.StatusCode = 401;
                return AuthenticateResult.Fail("Refresh token emapty");
            }

            if (!await jwtProvider.IsValidRefreshAsync(refreshToken))
                return AuthenticateResult.Fail("Refresh token is invalid");

            var accessToken = Request.Cookies["auction-access"];
            if (!string.IsNullOrEmpty(accessToken) && jwtProvider.IsValidAccess(accessToken))
                return Success(accessToken);          
            
            var newAccess = await jwtProvider.GenerateNewAccessTokenByRefresh(refreshToken);
            if (!string.IsNullOrEmpty(newAccess))
            {
                AppendAccessTokenToCookie(Response, newAccess);
                return Success(newAccess);
            }

            return AuthenticateResult.Fail("Refresh token is invalid");
        }

        private AuthenticateResult Success(string accessToken)
        {
            var identity = new ClaimsIdentity(jwtProvider.GetClaims(accessToken), Scheme.Name);
            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, Scheme.Name);
            return AuthenticateResult.Success(ticket);
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
