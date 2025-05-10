using Auction.Application.Common.Models;
using Auction.Application.Common.Models.Dto;
using Auction.Application.Features.Users.Commands.CreateUser;
using Auction.Application.Features.Users.Commands.Login;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Auction.WebApi.Controllers.Auth
{
    [Route("/api/[controller]")]
    public class AuthenticationController : BaseController
    {
        public AuthenticationController(IMediator mediator, IMapper mapper) : base(mediator, mapper)
        {
        }

        [HttpPost("/register")]
        public async Task<IActionResult> Register([FromBody] UserDto userDto)
        {
            var createResult = await _mediator.Send(_mapper.Map<CreateUserCommand>(userDto));

            if (!createResult.IsSuccess)
                return BadRequest(createResult.ErrorMessage);

            AppendTokenToCookie(Response, createResult.Data!.Token);

            return Ok(createResult.Data);
        }

        [HttpPost("/login")]
        public async Task<IActionResult> Login([FromBody] UserDto userDto)
        {
            var loginResult = await _mediator.Send(_mapper.Map<LoginUserCommand>(userDto));

            if (!loginResult.IsSuccess)
                return BadRequest(loginResult.ErrorMessage);

            return Ok(loginResult.Data);
        }

        private void AppendTokenToCookie(HttpResponse respone, string token)
        {
            respone.Cookies.Append("auction-token", token);
        }
    }
}
