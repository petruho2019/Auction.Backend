using Auction.Application.Common.Mappings;
using Auction.Application.Features.Users.Commands.CreateUser;
using AutoMapper;
using System.ComponentModel.DataAnnotations;

namespace Auction.Application.Common.Models.Dto.Users
{
    public class UserRegisterDto : UserRequest, IMapWith<CreateUserCommand>
    {
        [EmailAddress(ErrorMessage = "Неправильный формат email")]
        [Required(ErrorMessage = "Поле 'Почта' обязательно для заполнения")]
        public string Email { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<UserRegisterDto, CreateUserCommand>()
                .ForMember(command => command.Username, opt => opt.MapFrom(ud => ud.Username))
                .ForMember(command => command.Email, opt => opt.MapFrom(ud => ud.Email))
                .ForMember(command => command.Password, opt => opt.MapFrom(ud => ud.Password));
        }
    }
}
