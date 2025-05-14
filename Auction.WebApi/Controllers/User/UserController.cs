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
    public class UserController : BaseController
    {
        public UserController(IMediator mediator, IMapper mapper) : base(mediator, mapper)
        {
        }

        [Route("me")]
        [HttpGet]
        public async Task<IActionResult> GetMyInfo()
        {
            var userInfo = await _mediator.Send(new GetUserInfoQuery());

            return Ok(userInfo);
        }
    }
}
