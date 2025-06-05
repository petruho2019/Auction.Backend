using Auction.Application.Common.Models.Dto.Users;
using Auction.Application.Common.Models.Vm.Users.Auth;
using Auction.Application.Features.Tokens.Commands.CreateRefreshToken;
using Auction.Application.Features.Users.Commands.CreateUser;
using Auction.Application.Features.Users.Queries.Login;
using Auction.Application.Interfaces;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Auction.WebApi.Controllers.Auth
{
    [Route("/api/[controller]")]
    [ApiController]
    public class AuthController(IJwtProvider jwtProvider, IMediator mediator, IMapper mapper, IConfiguration configuration) : BaseController(mediator, mapper)
    {
        private readonly TimeSpan _expireRefresh = DateTime.Now.AddMonths(int.Parse(configuration["JwtSettings:RefreshExpiresMonths"]!)) - DateTime.Now;
        private readonly TimeSpan _expireAccess = DateTime.Now.AddMinutes(int.Parse(configuration["JwtSettings:AccessExpiresMinutes"]!)) - DateTime.Now;

        [HttpGet("check")]
        public IActionResult CheckAuth()
        {
            if (User.Identity?.IsAuthenticated == true)
                return Ok(new { authenticated = true });
            return Unauthorized(new { authenticated = false });
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserRegisterDto userDto)
        {
            var createUserResult = await mediator.Send(mapper.Map<CreateUserCommand>(userDto));
            if (!createUserResult.IsSuccess)
                return ToActionResultError(createUserResult.Error!);

            var createRefreshTokenResult = await mediator.Send(new CreateRefreshTokenCommand()
            {
                Ip = HttpContext.Connection.LocalIpAddress!.ToString(),
                UserId = createUserResult.Success!.Data!.UserId,
                SkipDeviceLimitCheck = false
            });
            if (!createRefreshTokenResult.IsSuccess)
                return ToActionResultError(createRefreshTokenResult.Error!);

            var accessToken = jwtProvider.GenerateAccessToken(createUserResult.Success!.Data);
            UpdateUserContext(HttpContext, accessToken);
            AppendTokensToCookie(Response, jwtProvider.GenerateAccessToken(createUserResult.Success!.Data), createRefreshTokenResult.Success!.Data);

            return ToActionResultSuccess(mapper.Map<UserVm>(createUserResult.Success.Data), createUserResult.Success.StatusCode);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginDto userDto)
        {
            var loginResult = await mediator.Send(mapper.Map<LoginUserQuery>(userDto));

            if (!loginResult.IsSuccess)
                return ToActionResultError(loginResult.Error!);

            var createRefreshTokenResult = await mediator.Send(new CreateRefreshTokenCommand()
            {
                Ip = HttpContext.Connection.LocalIpAddress!.ToString(),
                SkipDeviceLimitCheck = true,
                UserId = loginResult.Success!.Data.UserId,
                Username = loginResult.Success.Data.Username,
                Email = loginResult.Success.Data.Email
            });

            var accessToken = jwtProvider.GenerateAccessToken(loginResult.Success!.Data);
            UpdateUserContext(HttpContext, accessToken);
            AppendTokensToCookie(Response, accessToken, createRefreshTokenResult.Success!.Data);

            return ToActionResultSuccess(mapper.Map<UserVm>(loginResult.Success.Data), loginResult.Success.StatusCode);
        }

        [HttpPost("logout")]
        public IActionResult Logout()
        {
            DeleteAccessTokenFromCookie(Response);
            DeleteRefreshTokenFromCookie(Response);
            return Ok();
        }

        private void DeleteRefreshTokenFromCookie(HttpResponse response)
        {
            response.Cookies.Delete("auction-refresh", new CookieOptions()
            {
                Path = "/",
                SameSite = SameSiteMode.Lax,
                Secure = true,
                HttpOnly = true,
                MaxAge = _expireRefresh
            });
        }
        private void DeleteAccessTokenFromCookie(HttpResponse response)
        {
            response.Cookies.Delete("auction-access", new CookieOptions()
            {
                Path = "/",
                SameSite = SameSiteMode.None,
                Secure = true,
                HttpOnly = true,
                MaxAge = _expireAccess
            });
        }
        private void AppendTokensToCookie(HttpResponse response, string accessToken, string refreshToken)
        {
            AppendRefreshTokenToCookie(response, refreshToken);
            AppendAccessTokenToCookie(response, accessToken);
        }
        private void AppendRefreshTokenToCookie(HttpResponse response, string token)
        {
            response.Cookies.Append("auction-refresh", token, new CookieOptions()
            {
                SameSite = SameSiteMode.Lax,
                Secure = true,
                HttpOnly = true,
                MaxAge = _expireRefresh
            });
        }
        private void AppendAccessTokenToCookie(HttpResponse response, string token)
        {
            response.Cookies.Append("auction-access", token, new CookieOptions() { 
                SameSite = SameSiteMode.Lax, 
                Secure = true, 
                HttpOnly = true, 
                MaxAge = _expireAccess
            });
        }

        private void UpdateUserContext(HttpContext httpContext, string accessToken)
        {
            var claims = jwtProvider.GetClaims(accessToken);
            var identity = new ClaimsIdentity(claims, "Custom"); 
            httpContext.SignInAsync(new ClaimsPrincipal(identity));
        }
    }
}
