using Auction.Application.Common.Models.Dto.Auction;
using Auction.Application.Common.Models.Vm.Auctions.GetById;
using Auction.Application.Features.Auctions.Commands.CreateAuction;
using Auction.Application.Features.Auctions.Commands.EndAction;
using Auction.Application.Features.Auctions.Queries.GetById;
using Auction.Application.Features.Auctions.Queries.GetListAuction;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Auction.WebApi.Controllers.Auction
{
    [ApiController]
    [Route("/api/[controller]")]
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

            return Ok(createResult.Data);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var result = await _mediator.Send(new GetAuctionByIdQuery() { AuctionId = id });

            if (!result.IsSuccess)
                return BadRequest(result.ErrorMessage);

            return Ok(result.Data);
        }

        [HttpGet("")]
        public async Task<IActionResult> GetList()
        {
            var listAuctions = await _mediator.Send(new GetListAuctionsQuery());

            return Ok(listAuctions);
        }
    }
}
