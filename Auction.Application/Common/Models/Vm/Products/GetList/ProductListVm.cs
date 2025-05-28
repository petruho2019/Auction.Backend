using Auction.Application.Common.Mappings;
using Auction.Domain.Models;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auction.Application.Common.Models.Vm.Products.GetList
{
    public class ProductListVm : IMapWith<Product>
    {
        public Guid ProductId { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public string Location { get; set; }
        public int Quantity { get; set; }
        public DateTime DateCreate { get; set; } = DateTime.Now;
        public List<string> Images { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Product, ProductListVm>()
                .ForMember(plv => plv.ProductId, opt => opt.MapFrom(p => p.Id))
                .ForMember(plv => plv.Name, opt => opt.MapFrom(p => p.Name))
                .ForMember(plv => plv.Description, opt => opt.MapFrom(p => p.Description))
                .ForMember(plv => plv.Location, opt => opt.MapFrom(p => p.Location))
                .ForMember(plv => plv.Quantity, opt => opt.MapFrom(p => p.Quantity))
                .ForMember(plv => plv.DateCreate, opt => opt.MapFrom(p => p.Created))
                .ForMember(plv => plv.Images, opt => opt.MapFrom(p => p.Images.Select(i => i.Image).ToList()));
        }
    }
}
