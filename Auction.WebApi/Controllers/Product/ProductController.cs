using Auction.Application.Common.Models.Dto.Products;
using Auction.Application.Features.Products.Commands.CreateProduct;
using Auction.Application.Features.Products.Queries.GetAll;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Auction.WebApi.Controllers.Product
{
    [ApiController]
    [Route("/api/[controller]")]
    public class ProductController : BaseController
    {
        public ProductController(IMediator mediator, IMapper mapper) : base(mediator, mapper)
        {
        }

        [Route("create")]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateProductDto createProductDto)
        {
            var productVm = await _mediator.Send(_mapper.Map<CreateProductCommand>(createProductDto));

            return Ok(productVm);
        }

        [Route("/my")]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _mediator.Send(new GetListProductQuery()));
        }
    }
}
