using Auction.Application.Common.Mappings;
using Auction.Application.Features.Products.Commands;
using Auction.Domain.Models;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auction.Application.Common.Models.Vm.Products
{
    public class ProductVm : IMapWith<Product>
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Location { get; set; }
        public int Quantity { get; set; }
        public List<byte[]> Image { get; set; }
        public long Price { get; set; }
        public DateTime DateCreate { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Product, ProductVm>()
                .ForMember(vm => vm.Name, opt => opt.MapFrom(p => p.Name))
                .ForMember(vm => vm.Description, opt => opt.MapFrom(p => p.Description))
                .ForMember(vm => vm.Location, opt => opt.MapFrom(p => p.Location))
                .ForMember(vm => vm.Quantity, opt => opt.MapFrom(p => p.Quantity))
                .ForMember(vm => vm.Price, opt => opt.MapFrom(p => p.Price))
                .ForMember(vm => vm.DateCreate, opt => opt.MapFrom(p => p.DateCreate))
                .ForMember(vm => vm.Image, opt => opt.MapFrom(p => p.Images.Select(i => i.Image).ToList()));
        }
    }
}
