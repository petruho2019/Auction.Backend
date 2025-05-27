using Auction.Application.Common.Models.Vm.Products.GetList;
using Auction.Application.Features.Products.Queries.GetAll;
using Auction.Application.Interfaces;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Auction.Application.Features.Products.Queries.GetListProduct
{
    public class GetListProductQueryHanlder(IAuctionContext dbContext, IMapper mapper, ICurrentUserService currentUserService) : IRequestHandler<GetListProductQuery, List<ProductListVm>>
    {
        public async Task<List<ProductListVm>> Handle(GetListProductQuery request, CancellationToken cancellationToken)
        {
            var productsWithCalculateQuantity = await dbContext.Auctions
                .Include(a => a.Product)
                .Include(a => a.Product.Images)
                .Select(a => new ProductListVm
                {
                    ProductId = a.Product.Id,
                    Name = a.Product.Name,
                    Description = a.Product.Description,
                    Location = a.Product.Location,
                    Quantity = a.Product.Quantity - a.Product.Auctions.Sum(a => a.Quantity),
                    DateCreate = a.Product.Created,
                    Images = a.Product.Images.Select(i => i.Image).ToList()
                }).ToListAsync();

            var listVms = new List<ProductListVm>();

            foreach (var product in productsWithCalculateQuantity)
            {
                listVms.Add(mapper.Map<ProductListVm>(product));
            }

            return listVms;
        }
    }
}
