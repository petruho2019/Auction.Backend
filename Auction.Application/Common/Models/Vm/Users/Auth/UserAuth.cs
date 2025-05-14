using Auction.Application.Common.Mappings;
using Auction.Domain.Models;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auction.Application.Common.Models.Vm.Users.Auth
{
    public class UserAuth : IMapWith<User>
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string Token { get; set; } = string.Empty;

        public void Mapping(Profile profile)
        {
            profile.CreateMap<User, UserAuth>()
                .ForMember(ua => ua.Username, opt => opt.MapFrom(u => u.Username))
                .ForMember(ua => ua.Email, opt => opt.MapFrom(u => u.Email));
        }
    }
}
