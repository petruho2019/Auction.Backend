using Auction.Application.Attributes.Class.Filters;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Auction.WebApi.Controllers
{
    public class BaseController : ControllerBase
    {
        public readonly IMediator _mediator;
        public readonly IMapper _mapper;
        public BaseController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
            
        }

    }
}
