using Auction.Application.Common.Models;
using Auction.Application.Common.Models.Dto.Users;
using Auction.Application.Common.Models.Vm.Users.Auth;
using Auction.Application.Features.Users.Commands.CreateUser;
using Auction.Application.Features.Users.Commands.Login;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Auction.WebApi.Controllers.Auth
{
    [Route("/api/[controller]")]
    [ApiController]
    public class AuthController : BaseController
    {
        public AuthController(IMediator mediator, IMapper mapper) : base(mediator, mapper)
        {
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
            var createResult = await _mediator.Send(_mapper.Map<CreateUserCommand>(userDto));

            if (!createResult.IsSuccess)
                return BadRequest(createResult.ErrorMessage);

            AppendTokenToCookie(Response, createResult.Data!.Token);

            return Ok(_mapper.Map<UserVm>(createResult.Data));
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginDto userDto)
        {
            var loginResult = await _mediator.Send(_mapper.Map<LoginUserCommand>(userDto));

            if (!loginResult.IsSuccess)
                return BadRequest(loginResult.ErrorMessage);

            AppendTokenToCookie(Response, loginResult.Data!.Token);

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
