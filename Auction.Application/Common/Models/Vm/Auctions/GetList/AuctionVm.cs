using Auction.Application.Common.Mappings;
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
        public long CurrentPrice { get; set; }
        public DateTime DateStart { get; set; }
        public DateTime DateEnd { get; set; }
        public int Quantity { get; set; }
        public UserActionVm Creator { get; set; }
        public ProductAuctionVm Product { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Domain.Models.Auction, AuctionVm>()
                .ForMember(av => av.CurrentPrice, opt => opt.MapFrom(a => a.CurrentPrice))
                .ForMember(av => av.DateStart, opt => opt.MapFrom(a => a.DateStart))
                .ForMember(av => av.DateEnd, opt => opt.MapFrom(a => a.DateEnd))
                .ForMember(av => av.Quantity, opt => opt.MapFrom(a => a.Quantity))
                .ForPath(av => av.Creator.Username, opt => opt.MapFrom(a => a.Creator.Username))
                .ForPath(av => av.Creator.Email, opt => opt.MapFrom(a => a.Creator.Email))
                .ForPath(av => av.Product.Name, opt => opt.MapFrom(a => a.Product.Name))
                .ForPath(av => av.Product.Description, opt => opt.MapFrom(a => a.Product.Description))
                .ForPath(av => av.Product.Location, opt => opt.MapFrom(a => a.Product.Location))
                .ForPath(av => av.Product.Images, opt => opt.MapFrom(a => a.Product.Images.Select(i => i.Image).ToList()));


        }
    }
}
