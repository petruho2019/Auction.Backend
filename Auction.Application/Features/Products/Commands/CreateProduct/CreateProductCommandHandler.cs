using Auction.Application.Common.Models.Vm.Products;
using Auction.Application.Interfaces;
using Auction.Domain.Models;
using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auction.Application.Features.Products.Commands.CreateProduct
{
    public class CreateProductCommandHandler : BaseComponentHandler, IRequestHandler<CreateProductCommand, ProductVm>
    {

        public CreateProductCommandHandler(IAuctionContext dbContext, IMapper mapper, ICurrentUserService currentUserService) : base(dbContext, mapper, currentUserService)
        {
        }

        public async Task<ProductVm> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {

            var product = new Product()
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                Description = request.Description,
                Location = request.Location,
                Price = request.Price,
                Quantity = request.Quantity,
                UserId = _userCurrentService.UserId,
            };

            var images = new List<ProductImage>();

            foreach (var image in request.Images)
            {
                images.Add(new() { Image = image, Product = product });
            }

            product.Images = images;

            await _dbContext.Products.AddAsync(product, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return _mapper.Map<ProductVm>(product);
        }
    }
}
