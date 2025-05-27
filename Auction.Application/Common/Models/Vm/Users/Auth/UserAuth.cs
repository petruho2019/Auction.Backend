using Auction.Application.Common.Mappings;
using Auction.Domain.Models;
using AutoMapper;

namespace Auction.Application.Common.Models.Vm.Users.Auth
{
    public class UserAuth : IMapWith<User>
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public Guid UserId { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<User, UserAuth>()
                .ForMember(ua => ua.Username, opt => opt.MapFrom(u => u.Username))
                .ForMember(ua => ua.Email, opt => opt.MapFrom(u => u.Email))
                .ForMember(ua => ua.UserId, opt => opt.MapFrom(u => u.Id));
        }
    }
}
