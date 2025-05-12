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
    public class GetListProductQueryHanlder : BaseComponentHandler, IRequestHandler<GetListProductQuery, List<Product>>
    {
        public GetListProductQueryHanlder(IAuctionContext dbContext, IMapper mapper, ICurrentUserService currentUserService) : base(dbContext, mapper, currentUserService)
        {
        }

        public async Task<List<Product>> Handle(GetListProductQuery request, CancellationToken cancellationToken)
        {
            return await _dbContext.Products.Where(p => p.UserId == _userCurrentService.UserId).ToListAsync(cancellationToken);
        }
    }
}
