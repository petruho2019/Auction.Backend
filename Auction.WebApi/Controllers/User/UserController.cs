using Auction.Application.Features.Users.Queries.GetCurrentUserInfo;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Auction.WebApi.Controllers.User
{
    [ApiController]
    [Route("/api/[controller]")]
    [Authorize]
    public class UserController(IMediator mediator, IMapper mapper) : BaseController(mediator, mapper)
    {

        [Route("me")]
        [HttpGet]
        public async Task<IActionResult> GetMyInfo()
        {
            var userInfo = await mediator.Send(new GetUserInfoQuery());

            return Ok(userInfo);
        }
    }
}
