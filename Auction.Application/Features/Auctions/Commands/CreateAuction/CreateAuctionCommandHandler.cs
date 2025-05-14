using Auction.Application.Common.Models;
using Auction.Application.Common.Models.Vm.Auctions.Create;
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

namespace Auction.Application.Features.Auctions.Commands.CreateAuction
{
    class CreateAuctionCommandHandler : BaseComponentHandler, IRequestHandler<CreateAuctionCommand, Result<CreateAuctionVm>>
    {
        public CreateAuctionCommandHandler(IAuctionContext dbContext, IMapper mapper, ICurrentUserService currentUserService) : base(dbContext, mapper, currentUserService)
        {
        }

        public async Task<Result<CreateAuctionVm>> Handle(
        CreateAuctionCommand request,
        CancellationToken ct)
        {

            if (request.DateEnd < request.DateStart)
                return CreateFailureResult<CreateAuctionVm>(
                    "'Дата окончания' не может быть раньше 'Дата начала'");

            if (request.DateEnd > request.DateStart.AddYears(1))
                return CreateFailureResult<CreateAuctionVm>(
                    "Аукцион не может идти дольше 1 года");

            var productInfo = await _dbContext.Products
                .AsNoTracking()
                .Where(p => p.Id == request.ProductId)
                .Select(p => new
                {
                    p.Quantity,
                    UsedQuantity = _dbContext.Auctions
                        .Where(a => a.ProductId == p.Id)
                        .Sum(a => (int?)a.Quantity) ?? 0
                })
                .FirstOrDefaultAsync(ct);

            if (productInfo is null)
                return CreateFailureResult<CreateAuctionVm>("Продукта не существует");

            if (productInfo.Quantity - productInfo.UsedQuantity < request.Quantity)
                return CreateFailureResult<CreateAuctionVm>(
                    "Вы превысили количество выставляемых товаров");

            var creatorDto = await _dbContext.Users
                .AsNoTracking()
                .Where(u => u.Id == _userCurrentService.UserId)
                .Select(u => new { u.Id, u.Username, u.Email })
                .FirstOrDefaultAsync(ct);

            if (creatorDto is null)
                return CreateFailureResult<CreateAuctionVm>("Пользователь не найден");

            var auction = new Domain.Models.Auction
            {
                Id = Guid.NewGuid(),
                CreatorId = creatorDto.Id,
                Creator = new User { Id = creatorDto.Id, Username = creatorDto.Username, Email = creatorDto.Email },
                ProductId = request.ProductId,
                CurrentPrice = request.Price,
                DateStart = request.DateStart,
                DateEnd = request.DateEnd,
                Quantity = request.Quantity
            };

            await _dbContext.Auctions.AddAsync(auction, ct);
            await _dbContext.SaveChangesAsync(ct);

            var vm = _mapper.Map<CreateAuctionVm>(auction);
            return CreateSuccessResult(vm);
        }

    }
}
