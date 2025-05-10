using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Auction.WebApi.Controllers.Auth
{
    [Route("/api/authorize")]
    [Authorize]
    public class AuhorizeTestController : BaseController
    {
        public AuhorizeTestController(IMediator mediator, IMapper mapper) : base(mediator, mapper)
        {
        }

        [HttpGet("hello")]
        public IActionResult AuthorizeTest()
        {
            return Ok("Вы авторизированы");
        }
    }
}
