﻿using Auction.Application.Common.Mappings;
using AutoMapper;

namespace Auction.Application.Common.Models.Vm.Auctions.Create
{
    public class CreateAuctionVm : IMapWith<Domain.Models.Auction>
    {
        public Guid Id { get; set; }
        public double CurrentPrice { get; set; }
        public DateTime DateStart { get; set; }
        public DateTime DateEnd { get; set; }
        public int Quantity { get; set; }
        public ProductCreateAuctionVm Product { get; set; }
        public UserAuctionVm Creator { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Domain.Models.Auction, CreateAuctionVm>()
                .ForMember(vm => vm.Id, opt => opt.MapFrom(a => a.Id))
                .ForMember(vm => vm.CurrentPrice, opt => opt.MapFrom(a => a.CurrentPrice))
                .ForMember(vm => vm.DateStart, opt => opt.MapFrom(a => a.Start))
                .ForMember(vm => vm.DateEnd, opt => opt.MapFrom(a => a.End))
                .ForMember(vm => vm.Quantity, opt => opt.MapFrom(a => a.Quantity))
                .ForPath(vm => vm.Product.Name, opt => opt.MapFrom(a => a.Product.Name))
                .ForPath(vm => vm.Product.Location, opt => opt.MapFrom(a => a.Product.Location))
                .ForPath(vm => vm.Product.Description, opt => opt.MapFrom(a => a.Product.Description))
                .ForPath(vm => vm.Product.Images, opt => opt.MapFrom(a => a.Product.Images.Select(i => i.Image).ToList()))
                .ForPath(vm => vm.Creator.Username, opt => opt.MapFrom(a => a.Creator.Username))
                .ForPath(vm => vm.Creator.Email, opt => opt.MapFrom(a => a.Creator.Email));
        }
    }
}
