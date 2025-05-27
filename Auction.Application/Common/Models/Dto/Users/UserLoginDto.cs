using Auction.Application.Common.Mappings;
using Auction.Application.Common.Models.Vm.Users.Auth;
using Auction.Application.Features.Users.Commands.Login;
using AutoMapper;

namespace Auction.Application.Common.Models.Dto.Users
{
    public class UserLoginDto : UserRequest, IMapWith<UserAuth>
    {
        public void Mapping(Profile profile)
        {
            profile.CreateMap<UserLoginDto, LoginUserCommand>()
                .ForMember(command => command.Username, opt => opt.MapFrom(u => u.Username))
                .ForMember(command => command.Password, opt => opt.MapFrom(u => u.Password));
        }
    }
}
