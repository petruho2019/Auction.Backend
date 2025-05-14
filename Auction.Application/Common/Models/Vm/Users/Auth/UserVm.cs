using Auction.Application.Common.Mappings;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auction.Application.Common.Models.Vm.Users.Auth
{
    public class UserVm : IMapWith<UserAuth>
    {
        public string Username { get; set; }
        public string Email { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<UserAuth, UserVm>()
                .ForMember(uv => uv.Username, opt => opt.MapFrom(ua => ua.Username))
                .ForMember(uv => uv.Email, opt => opt.MapFrom(ua => ua.Email));
        }
    }
}
