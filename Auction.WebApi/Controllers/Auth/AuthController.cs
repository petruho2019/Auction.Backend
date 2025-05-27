using Auction.Application.Common.Extensions;
using Auction.Application.Common.Models.Dto.Users;
using Auction.Application.Common.Models.Vm.Users.Auth;
using Auction.Application.Features.Tokens.Commands.CreateRefreshToken;
using Auction.Application.Features.Users.Commands.CreateUser;
using Auction.Application.Features.Users.Commands.Login;
using Auction.Application.Interfaces;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Auction.WebApi.Controllers.Auth
{
    [Route("/api/[controller]")]
    [ApiController]
    public class AuthController(IJwtProvider jwtProvider, IMediator mediator, IMapper mapper) : BaseController(mediator, mapper)
    {
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
                return ToActionResultError(createUserResult.Error);

            await mediator.Send(new CreateRefreshTokenCommand()
            {
                Ip = HttpContext.Connection.LocalIpAddress!.ToString(),
                UserId = createUserResult.Success!.Data!.UserId,
                SkipDeviceLimitCheck = true
            });

            AppendTokenToCookie(Response, jwtProvider.GenerateToken(createUserResult.Success.Data));

            return ToActionResultSuccess(mapper.Map<UserVm>(createUserResult.Success.Data), createUserResult.Success.StatusCode);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginDto userDto)
        {
            var loginResult = await mediator.Send(mapper.Map<LoginUserCommand>(userDto));

            if (!loginResult.IsSuccess)
                return ToActionResultError(loginResult.Error);

            AppendTokenToCookie(Response, jwtProvider.GenerateToken(loginResult.Success.Data));

            return ToActionResultSuccess(mapper.Map<UserVm>(loginResult.Success.Data), loginResult.Success.StatusCode);
        }

        [HttpPost("logout")]
        public IActionResult Logout()
        {
            DeleteTokenFromCookie(Response);
            return Ok();
        }

        private void DeleteTokenFromCookie(HttpResponse response)
        {
            response.Cookies.Delete("auction-access", new CookieOptions()
            {
                Path = "/",
                SameSite = SameSiteMode.None,
                Secure = true,
                HttpOnly = true,
                MaxAge = TimeSpan.FromHours(24)
            });
        }

        private void AppendTokenToCookie(HttpResponse respone, string token)
        {
            respone.Cookies.Append("auction-token", token, new CookieOptions() { 
                SameSite = SameSiteMode.None, 
                Secure = true, 
                HttpOnly = true, 
                MaxAge = TimeSpan.FromHours(24) 
            });
        }
    }
}
