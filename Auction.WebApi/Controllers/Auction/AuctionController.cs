using Auction.Application.Common.Models.Dto.Auction;
using Auction.Application.Features.Auctions.Commands;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Auction.WebApi.Controllers.Auction
{
    [ApiController]
    [Route("/api/[controller]/")]
    [Authorize]
    public class AuctionController : BaseController
    {
        public AuctionController(IMediator mediator, IMapper mapper) : base(mediator, mapper)
        {
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] CreateAuctionDto dto)
        {
            var createResult = await _mediator.Send(_mapper.Map<CreateAuctionCommand>(dto));

            if (!createResult.IsSuccess)
            {
                return BadRequest(createResult.ErrorMessage);
            }

            return Ok(createResult);
        }
    }
}
