using Auction.Application.Common.Models.Vm.Products.GetList;
using Auction.Application.Features.Products.Queries.GetAll;
using Auction.Application.Interfaces;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Auction.Application.Features.Products.Queries.GetListProduct
{
    public class GetListProductsHanlder(IAuctionContext dbContext, IMapper mapper, ICurrentUserService currentUserService) : IRequestHandler<GetListProductsQuery, List<ProductListVm>>
    {
        public async Task<List<ProductListVm>> Handle(GetListProductsQuery request, CancellationToken cancellationToken)
        {
            // 1. Получаем данные из базы без лишних преобразований
            var products = await dbContext.Products
                .Include(p => p.Images)
                .Include(p => p.Auctions)
                .Where(p => p.UserId.Equals(currentUserService.UserId))
                .ToListAsync(cancellationToken);

            // 2. Вручную маппим в ViewModel с вычислениями
            var result = products.Select(p => new ProductListVm
            {
                ProductId = p.Id,
                Name = p.Name,
                Description = p.Description,
                Location = p.Location,
                Quantity = p.Quantity - p.Auctions.Sum(a => a.Quantity),
                DateCreate = p.Created,
                Images = p.Images?.Select(i => i.Image).ToList() ?? new List<string>()
            }).ToList();

            return result;
        }
    }
}
