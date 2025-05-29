using Auction.Application.Common.Models.Vm.Products.GetList;
using MediatR;

namespace Auction.Application.Features.Products.Queries.GetAll
{
    public record GetListProductsQuery : IRequest<List<ProductListVm>>;
}
