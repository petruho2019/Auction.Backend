using Auction.Application.Common.Models;
using Auction.Application.Common.Models.Vm.Auctions;
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

namespace Auction.Application.Features.Auctions.Commands
{
    class CreateAuctionCommandHandler : BaseComponentHandler, IRequestHandler<CreateAuctionCommand, Result<CreateAuctionVm>>
    {
        public CreateAuctionCommandHandler(IAuctionContext dbContext, IMapper mapper, ICurrentUserService currentUserService) : base(dbContext, mapper, currentUserService)
        {
        }

        public async Task<Result<CreateAuctionVm>> Handle(CreateAuctionCommand request, CancellationToken cancellationToken)
        {
            if (_dbContext.Products.Any(p => p.Id == request.ProductId))
                return CreateFailureResult<CreateAuctionVm>("Продукта не существует");

            if ((request.DateStart - request.DateEnd).TotalDays > 365)
                return CreateFailureResult<CreateAuctionVm>("Аукцион не может идти дольше 1 года");

            if ((request.DateStart - request.DateEnd) < TimeSpan.Zero)
                return CreateFailureResult<CreateAuctionVm>("'Дата окончания' не может быть раньше 'Дата начала'");


            var allQuantity = _dbContext.Products
                .Include(p => p.Auctions)
                .Where(p => p.Id == request.ProductId)
                .ToList()
                .Sum(p => p.Quantity);

            if (request.Quantity - allQuantity < 0)
                return CreateFailureResult<CreateAuctionVm>("Вы привысили количество выставляемых товаров");

            var auction = new Domain.Models.Auction()
            {
                Id = Guid.NewGuid(),
                CreatorId = _userCurrentService.UserId,
                CurrentPrice = request.Price,
                DateStart = request.DateStart,
                DateEnd = request.DateEnd,
                ProductId = request.ProductId,
                Quantity = request.Quantity
            };

            await _dbContext.Auctions.AddAsync(auction, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return CreateSuccessResult(_mapper.Map<CreateAuctionVm>(auction));
        }
    }
}
