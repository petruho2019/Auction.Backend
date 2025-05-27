using Auction.Application.Common.Extensions;
using Auction.Application.Common.Models;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Auction.WebApi.Controllers
{
    public class BaseController(IMediator mediator, IMapper mapper) : ControllerBase
    {
        [ApiExplorerSettings(IgnoreApi = true)]
        public IActionResult ToActionResultSuccess<T>(Success<T> success)
            => new ObjectResult(success.Data) { StatusCode = success.StatusCode.GetInt() };
        [ApiExplorerSettings(IgnoreApi = true)]
        public IActionResult ToActionResultSuccess<T>(T data, HttpStatusCode status)
            => new ObjectResult(data) { StatusCode = status.GetInt() };
        [ApiExplorerSettings(IgnoreApi = true)]
        public IActionResult ToActionResultError(Error error)
           => new ObjectResult(error.ErrorMessage) { StatusCode = error.StatusCode.GetInt() };
    }
}
