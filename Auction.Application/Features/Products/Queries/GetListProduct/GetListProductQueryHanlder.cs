using Auction.Application.Common.Exceptions;
using Auction.Application.Common.Models;
using Auction.Application.Common.Models.Vm.Products;
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
            var products = await _dbContext.Products
                .Include(p => p.Images)
                .Where(p => p.UserId == _userCurrentService.UserId)
                .ToListAsync(cancellationToken);

            var listVms = new List<ProductListVm>();

            foreach (var product in products)
            {
                listVms.Add(_mapper.Map<ProductListVm>(product));
            }

            return listVms;
        }
    }
}
