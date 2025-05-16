
using Auction.Application.Common.Models.Vm.Products.GetList;
using Auction.Application.Features.Products.Queries.GetAll;
using Auction.Application.Interfaces;
using Auction.Domain.Models;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auction.Application.Features.Products.Queries.GetListProduct
{
    public class GetListProductQueryHanlder : BaseComponentHandler, IRequestHandler<GetListProductQuery, List<ProductListVm>>
    {
        public GetListProductQueryHanlder(IAuctionContext dbContext, IMapper mapper, ICurrentUserService currentUserService) : base(dbContext, mapper, currentUserService)
        {
        }

        public async Task<List<ProductListVm>> Handle(GetListProductQuery request, CancellationToken cancellationToken)
        {
            var productsWithCalculateQuantity = await _dbContext.Auctions
                .Include(a => a.Product)
                .Include(a => a.Product.Images)
                .Select(a => new ProductListVm
                {
                    ProductId = a.Product.Id,
                    Name = a.Product.Name,
                    Description = a.Product.Description,
                    Location = a.Product.Location,
                    Quantity = a.Product.Quantity - a.Product.Auctions.Sum(a => a.Quantity),
                    DateCreate = a.Product.DateCreate,
                    Images = a.Product.Images.Select(i => i.Image).ToList()
                }).ToListAsync();

            var listVms = new List<ProductListVm>();

            foreach (var product in productsWithCalculateQuantity)
            {
                listVms.Add(_mapper.Map<ProductListVm>(product));
            }

            return listVms;
        }
    }
}
