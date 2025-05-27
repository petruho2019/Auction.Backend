using Auction.Application.Common.Extensions;
using Auction.Application.Common.Models.Dto.Auction;
using Auction.Application.Common.Models.Vm.Auctions.Create;
using Auction.Application.Common.Models.Vm.Auctions.GetById;
using Auction.Application.Features.Auctions.Commands.CreateAuction;
using Auction.Application.Features.Auctions.Commands.EndAction;
using Auction.Application.Features.Auctions.Queries.GetById;
using Auction.Application.Features.Auctions.Queries.GetListAuction;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Auction.WebApi.Controllers.Auction
{
    [ApiController]
    [Route("/api/[controller]")]
    [Authorize]
    public class AuctionController(IMediator mediator, IMapper mapper) : BaseController(mediator, mapper)
    {

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] CreateAuctionDto dto)
        {
            var createResult = await mediator.Send(mapper.Map<CreateAuctionCommand>(dto));

            if (!createResult.IsSuccess)
            {
                return ToActionResultError(createResult.Error);
            }

            return ToActionResultSuccess(createResult.Success);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var result = await mediator.Send(new GetAuctionByIdQuery() { AuctionId = id });

            if (!result.IsSuccess)
                return ToActionResultError(result.Error);

            return ToActionResultSuccess(result.Success);
        }

        [HttpGet("")]
        public async Task<IActionResult> GetList()
        {
            Console.WriteLine(Activity.Current.GetTagItem("UserId"));

            var listAuctions = await mediator.Send(new GetListAuctionsQuery());

            return Ok(listAuctions);
        }
    }
}
