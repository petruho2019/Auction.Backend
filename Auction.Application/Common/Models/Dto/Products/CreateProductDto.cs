
using Auction.Application.Common.Attributes.Property.CityValidation;
using Auction.Application.Common.Attributes.Property.Integer;
using Auction.Application.Common.Mappings;
using Auction.Application.Features.Products.Commands.CreateProduct;
using AutoMapper;
using System.ComponentModel.DataAnnotations;

namespace Auction.Application.Common.Models.Dto.Products
{
    public class CreateProductDto : IMapWith<CreateProductCommand>
    {
        [Required(ErrorMessage = "Поле 'Название' обязательно для заполнения")]
        [MaxLength(20, ErrorMessage = "'Название' должено содержать не больше 20 символов")]
        [MinLength(3, ErrorMessage = "'Название' должено содержать не менее 3 символов")]
        public string Name { get; set; }

        [MaxLength(300, ErrorMessage = "Описание должено содержать не больше 300 символов")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Поле 'Расположение' обязательно для заполнения")]
        [CityValidator]
        public string Location { get; set; }

        [NotZero]
        [Range(1, 200, ErrorMessage = "Количество не должно превышать 200 единииц")]
        public int Quantity { get; set; }

        [Required(ErrorMessage = "Поле 'Изображения' не должно быть пустым")]
        public List<byte[]> Images { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<CreateProductDto, CreateProductCommand>()
                .ForMember(command => command.Name, opt => opt.MapFrom(dto => dto.Name))
                .ForMember(command => command.Description, opt => opt.MapFrom(dto => dto.Description))
                .ForMember(command => command.Location, opt => opt.MapFrom(dto => dto.Location))
                .ForMember(command => command.Quantity, opt => opt.MapFrom(dto => dto.Quantity))
                .ForMember(command => command.Images, opt => opt.MapFrom(dto => dto.Images));
        }
    }
}
