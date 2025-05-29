using Auction.Application.Common.Mappings;
using Auction.Domain.Models;
using AutoMapper;

namespace Auction.Application.Common.Models.Vm.Products.Create
{
    public class CreateProductVm : IMapWith<Product>
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Location { get; set; }
        public int Quantity { get; set; }
        public List<string> Image { get; set; }
        public DateTime DateCreate { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Product, CreateProductVm>()
                .ForMember(vm => vm.Name, opt => opt.MapFrom(p => p.Name))
                .ForMember(vm => vm.Description, opt => opt.MapFrom(p => p.Description))
                .ForMember(vm => vm.Location, opt => opt.MapFrom(p => p.Location))
                .ForMember(vm => vm.Quantity, opt => opt.MapFrom(p => p.Quantity))
                .ForMember(vm => vm.DateCreate, opt => opt.MapFrom(p => p.Created))
                .ForMember(vm => vm.Image, opt => opt.MapFrom(p => p.Images.Select(i => i.Image).ToList()));
        }
    }
}
