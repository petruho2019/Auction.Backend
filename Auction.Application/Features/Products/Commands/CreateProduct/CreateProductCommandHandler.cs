using Auction.Application.Common.Models.Vm.Products.Create;
using Auction.Application.Interfaces;
using Auction.Domain.Models;
using AutoMapper;
using MediatR;

namespace Auction.Application.Features.Products.Commands.CreateProduct
{
    public class CreateProductCommandHandler(IAuctionContext dbContext, IMapper mapper, ICurrentUserService currentUserService) : IRequestHandler<CreateProductCommand, CreateProductVm>
    {
        public async Task<CreateProductVm> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {

            var product = new Product()
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                Description = request.Description,
                Location = request.Location,
                Quantity = request.Quantity,
                UserId = currentUserService.UserId,
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

            await dbContext.Products.AddAsync(product, cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);

            return mapper.Map<CreateProductVm>(product);
        }
    }
}
