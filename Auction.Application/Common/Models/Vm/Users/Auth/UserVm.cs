using Auction.Application.Common.Mappings;
using Auction.Domain.Models;
using AutoMapper;

namespace Auction.Application.Common.Models.Vm.Users.Auth
{
    public class UserVm : IMapWith<User>
    {
        public string Username { get; set; }
        public string Email { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<UserAuth, UserVm>();
        }
    }
}
