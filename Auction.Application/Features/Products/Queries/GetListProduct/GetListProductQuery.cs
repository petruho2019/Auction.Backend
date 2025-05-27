using Auction.Application.Common.Models.Vm.Products.GetList;
using MediatR;

namespace Auction.Application.Features.Products.Queries.GetAll
{
    public record GetListProductQuery : IRequest<List<ProductListVm>>;
}
