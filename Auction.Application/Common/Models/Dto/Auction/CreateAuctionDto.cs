
using Auction.Application.Common.Attributes.Property.Date;
using Auction.Application.Common.Attributes.Property.Integer;
using Auction.Application.Common.Mappings;
using Auction.Application.Features.Auctions.Commands.CreateAuction;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auction.Application.Common.Models.Dto.Auction
{
    public class CreateAuctionDto : IMapWith<CreateAuctionCommand>
    {
        [Required(ErrorMessage = "Поле 'Цена' обязательно для заполнения")]
        [NotZero]
        public double Price { get; set; }

        [Required(ErrorMessage = "Поле 'Дата начала' обязательно для заполнения")]
        [ValidStartDate]
        public DateTime DateStart { get; set; }

        [Required(ErrorMessage = "Поле 'Дата окончания' обязательно для заполнения")]
        public DateTime DateEnd { get; set; }

        [Required(ErrorMessage = "Поле 'Количество' обязательно для заполнения")]
        [NotZero]
        public int Quantity { get; set; }

        [Required(ErrorMessage = "ProductId is required!")]
        public Guid ProductId { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<CreateAuctionDto, CreateAuctionCommand>()
                .ForMember(command => command.Price, opt => opt.MapFrom(dto => dto.Price))
                .ForMember(command => command.DateStart, opt => opt.MapFrom(dto => dto.DateStart))
                .ForMember(command => command.DateEnd, opt => opt.MapFrom(dto => dto.DateEnd))
                .ForMember(command => command.Quantity, opt => opt.MapFrom(dto => dto.Quantity))
                .ForMember(command => command.ProductId, opt => opt.MapFrom(dto => dto.ProductId));
        }
    }
}
