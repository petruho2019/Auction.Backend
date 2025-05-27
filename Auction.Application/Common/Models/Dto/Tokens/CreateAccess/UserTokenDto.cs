using Auction.Application.Common.Mappings;
using Auction.Application.Common.Models.Vm.Users.Auth;
using AutoMapper;

namespace Auction.Application.Common.Models.Dto.Tokens.CreateAccess
{
    public class UserTokenDto : IMapWith<UserAuth>
    {
        public string Username { get; set; }
        public Guid Id { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<UserAuth, UserTokenDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.UserId));
        }
    }
}
