using Auction.Application.Common.Mappings;
using Auction.Application.Common.Models.Vm;
using Auction.Application.Features.Users.Commands.Login;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auction.Application.Common.Models.Dto
{
    public class UserLoginDto : IMapWith<UserVm>
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public void Mapping(Profile profile)
        {
            profile.CreateMap<UserLoginDto, LoginUserCommand>()
                .ForMember(command => command.Username, opt => opt.MapFrom(u => u.Username))
                .ForMember(command => command.Password, opt => opt.MapFrom(u => u.Password));
        }
    }
}
