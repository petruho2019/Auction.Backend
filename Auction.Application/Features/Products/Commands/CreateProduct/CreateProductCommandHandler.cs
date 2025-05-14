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
    public class CreateProductCommandHandler : BaseComponentHandler, IRequestHandler<CreateProductCommand, CreateProductVm>
    {

        public CreateProductCommandHandler(IAuctionContext dbContext, IMapper mapper, ICurrentUserService currentUserService) : base(dbContext, mapper, currentUserService)
        {
        }

        public async Task<CreateProductVm> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {

            var product = new Product()
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                Description = request.Description,
                Location = request.Location,
                Quantity = request.Quantity,
                UserId = _userCurrentService.UserId,
            };

            var images = new List<ProductImage>();

            int count = 0;

            foreach (var image in request.Images)
            {
                count++;

                var imagesDir = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images");

                if (!Directory.Exists(imagesDir))
                    Directory.CreateDirectory(imagesDir);

                var fileName = $"{product.Id}_{count}.jpg";
                var filePath = Path.Combine(imagesDir, fileName);

                await File.WriteAllBytesAsync(filePath, image, cancellationToken);

                images.Add(new ProductImage
                {
                    Image = filePath,
                    Product = product
                });
            }

            product.Images = images;

            await _dbContext.Products.AddAsync(product, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return _mapper.Map<CreateProductVm>(product);
        }
    }
}
