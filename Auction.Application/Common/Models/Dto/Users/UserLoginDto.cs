using Auction.Application.Common.Mappings;
using Auction.Application.Common.Models.Vm.Users;
using Auction.Application.Features.Users.Commands.Login;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auction.Application.Common.Models.Dto.Users
{
    public class UserLoginDto : UserRequest, IMapWith<UserVm>
    {
        public void Mapping(Profile profile)
        {
            profile.CreateMap<UserLoginDto, LoginUserCommand>()
                .ForMember(command => command.Username, opt => opt.MapFrom(u => u.Username))
                .ForMember(command => command.Password, opt => opt.MapFrom(u => u.Password));
        }
    }
}
