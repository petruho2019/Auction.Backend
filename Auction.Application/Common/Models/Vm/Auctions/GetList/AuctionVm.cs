using Auction.Application.Common.Mappings;
using Auction.Application.Common.Models.Vm.Auctions.Create;
using Auction.Domain.Models;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auction.Application.Common.Models.Vm.Auctions.GetList
{
    public class AuctionVm : IMapWith<Domain.Models.Auction>
    {
        public Guid AuctionId { get; set; }
        public double CurrentPrice { get; set; }
        public DateTime DateStart { get; set; }
        public DateTime DateEnd { get; set; }
        public int Quantity { get; set; }
        public bool IsEnded { get; set; }
        public UserAuctionVm Creator { get; set; }
        public ProductCreateAuctionVm Product { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Domain.Models.Auction, AuctionVm>()
                .ForMember(vm => vm.AuctionId, opt => opt.MapFrom(a => a.Id))
                .ForMember(vm => vm.CurrentPrice, opt => opt.MapFrom(a => a.CurrentPrice))
                .ForMember(vm => vm.DateStart, opt => opt.MapFrom(a => a.DateStart))
                .ForMember(vm => vm.DateEnd, opt => opt.MapFrom(a => a.DateEnd))
                .ForMember(vm => vm.Quantity, opt => opt.MapFrom(a => a.Quantity))
                .ForMember(vm => vm.IsEnded, opt => opt.MapFrom(a => a.IsEnded))
                .ForPath(vm => vm.Creator.Username, opt => opt.MapFrom(a => a.Creator.Username))
                .ForPath(vm => vm.Creator.Email, opt => opt.MapFrom(a => a.Creator.Email))
                .ForPath(vm => vm.Product.Name, opt => opt.MapFrom(a => a.Product.Name))
                .ForPath(vm => vm.Product.Description, opt => opt.MapFrom(a => a.Product.Description))
                .ForPath(vm => vm.Product.Location, opt => opt.MapFrom(a => a.Product.Location))
                .ForPath(vm => vm.Product.Images, opt => opt.MapFrom(a => a.Product.Images.Select(i => i.Image).ToList()));


        }
    }
}
