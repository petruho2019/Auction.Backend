using Auction.Application.Common.Mappings;
using Auction.Application.Common.Models.Vm;
using Auction.Application.Features.Users.Commands.CreateUser;
using Auction.Application.Features.Users.Commands.Login;
using Auction.Domain.Models;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auction.Application.Common.Models.Dto
{
    public class UserRegisterDto : IMapWith<UserVm>
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<UserRegisterDto, CreateUserCommand>()
                .ForMember(command => command.Username, opt => opt.MapFrom(ud => ud.Username))
                .ForMember(command => command.Email, opt => opt.MapFrom(ud => ud.Email))
                .ForMember(command => command.Password, opt => opt.MapFrom(ud => ud.Password));
        }
    }
}
