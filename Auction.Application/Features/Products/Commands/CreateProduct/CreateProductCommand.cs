using Auction.Application.Common.Models.Vm.Products;
using MediatR;

namespace Auction.Application.Features.Products.Commands.CreateProduct
{
    public class CreateProductCommand : IRequest<CreateProductVm>
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Location { get; set; }
        public int Quantity { get; set; }
        public List<byte[]> Images { get; set; }
        public long Price { get; set; }
    }
}
