using Auction.Application.Common.Models;
using Auction.Application.Common.Models.Vm.Auctions.Create;
using Auction.Application.Interfaces;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Auction.Application.Features.Auctions.Commands.CreateAuction
{
    class CreateAuctionCommandHandler(IAuctionContext dbContext, IMapper mapper, ICurrentUserService currentUserService) : IRequestHandler<CreateAuctionCommand, Result<CreateAuctionVm>>
    {

        public async Task<Result<CreateAuctionVm>> Handle(
        CreateAuctionCommand request,
        CancellationToken ct)
        {
            if (request.Price < 0)
                return Result<CreateAuctionVm>.BadRequest(
                    "'Цена' не может быть меньше либо равнятся 0");

            if (request.DateEnd < request.DateStart)
                return Result<CreateAuctionVm>.BadRequest(
                    "'Дата окончания' не может быть раньше 'Дата начала'");

            if (request.DateEnd > request.DateStart.AddYears(1))
                return Result<CreateAuctionVm>.BadRequest(
                    "Аукцион не может идти дольше 1 года");

            var productInfo = await dbContext.Products
                .AsNoTracking()
                .Where(p => p.Id.Equals(request.ProductId) && p.UserId.Equals(currentUserService.UserId))
                .Select(p => new
                {
                    p.Quantity,
                    UsedQuantity = dbContext.Auctions
                        .Where(a => a.ProductId == p.Id)
                        .Sum(a => (int?)a.Quantity) ?? 0
                })
                .FirstOrDefaultAsync(ct);

            if (productInfo is null)
                return Result<CreateAuctionVm>.BadRequest("Продукта не существует");

            if (productInfo.Quantity - productInfo.UsedQuantity < request.Quantity)
                return Result<CreateAuctionVm>.BadRequest(
                    "Вы превысили количество выставляемых товаров");

            var creatorDto = await dbContext.Users
                .AsNoTracking()
                .Where(u => u.Id == currentUserService.UserId)
                .Select(u => new { u.Id, u.Username, u.Email })
                .FirstOrDefaultAsync(ct);

            if (creatorDto is null)
                return Result<CreateAuctionVm>.BadRequest("Пользователь не найден");

            DateTime unspecifiedStart = DateTime.SpecifyKind(request.DateStart, DateTimeKind.Unspecified);
            DateTime unspecifiedEnd = DateTime.SpecifyKind(request.DateEnd, DateTimeKind.Unspecified);

            var auction = new Domain.Models.Auction
            {
                Id = Guid.NewGuid(),
                CreatorId = creatorDto.Id,
                ProductId = request.ProductId,
                CurrentPrice = request.Price,
                Start = unspecifiedStart,
                End = unspecifiedEnd,
                Quantity = request.Quantity
            };


            await dbContext.Auctions.AddAsync(auction, ct);
            await dbContext.SaveChangesAsync(ct);

            var vm = mapper.Map<CreateAuctionVm>(auction);
            vm.Creator = new() { Email = creatorDto.Email, Username = creatorDto.Username };
            return Result< CreateAuctionVm>.Created(vm);
        }

    }
}
