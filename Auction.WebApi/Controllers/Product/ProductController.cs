using Auction.Application.Common.Models.Dto.Products;
using Auction.Application.Features.Products.Commands.CreateProduct;
using Auction.Application.Features.Products.Queries.GetAll;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Auction.WebApi.Controllers.Product
{
    [ApiController]
    [Route("/api/[controller]")]
    [Authorize]
    public class ProductController(IMediator mediator, IMapper mapper) : BaseController(mediator, mapper)
    {

        [Route("create")]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateProductDto createProductDto)
        {
            var productVm = await mediator.Send(mapper.Map<CreateProductCommand>(createProductDto));

            return Ok(productVm);
        }

        [Route("list")]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var products = await mediator.Send(new GetListProductsQuery());
            return Ok(products);
        }
    }
}
