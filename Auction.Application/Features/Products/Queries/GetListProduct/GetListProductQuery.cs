using Auction.Application.Common.Models.Vm.Products;
using Auction.Domain.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auction.Application.Features.Products.Queries.GetAll
{
    public class GetListProductQuery : IRequest<List<ProductListVm>>
    {
    }
}
