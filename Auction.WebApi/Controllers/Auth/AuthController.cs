using Auction.Application.Common.Models;
using Auction.Application.Common.Models.Dto.Users;
using Auction.Application.Common.Models.Vm.Users.Auth;
using Auction.Application.Features.Tokens.Commands.CreateRefreshToken;
using Auction.Application.Features.Users.Commands.CreateUser;
using Auction.Application.Features.Users.Commands.Login;
using Auction.Application.Interfaces;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Auction.WebApi.Controllers.Auth
{
    [Route("/api/[controller]")]
    [ApiController]
    [Authorize]
    public class AuthController : BaseController
    {
        private readonly IJwtProvider _jwtProvider;
        public AuthController(IJwtProvider jwtProvider, IMediator mediator, IMapper mapper) : base(mediator, mapper)
        {
            _jwtProvider = jwtProvider;
        }

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
            var createUserResult = await _mediator.Send(_mapper.Map<CreateUserCommand>(userDto));

            if (!createUserResult.IsSuccess)
                return BadRequest(createUserResult.ErrorMessage);

            await _mediator.Send(new CreateRefreshTokenCommand()
            {
                Ip = HttpContext.Connection.LocalIpAddress!.ToString(),
                UserId = createUserResult.Data!.UserId,
                SkipDeviceLimitCheck = true
            });

            AppendTokenToCookie(Response, _jwtProvider.GenerateToken(createUserResult.Data));

            return Ok(_mapper.Map<UserVm>(createUserResult.Data));
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginDto userDto)
        {
            var loginResult = await _mediator.Send(_mapper.Map<LoginUserCommand>(userDto));

            if (!loginResult.IsSuccess)
                return BadRequest(loginResult.ErrorMessage);

            AppendTokenToCookie(Response, _jwtProvider.GenerateToken(loginResult.Data));

            return Ok(_mapper.Map<UserVm>(loginResult.Data));
        }

        [HttpPost("logout")]
        public IActionResult Logout()
        {
            DeleteTokenFromCookie(Response);
            return Ok();
        }

        private void DeleteTokenFromCookie(HttpResponse response)
        {
            response.Cookies.Delete("auction-token", new CookieOptions()
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
