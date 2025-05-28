using Auction.Application.Common.Mappings;
using Auction.Application.Common.Models;
using Auction.Application.Common.Models.Dto.Tokens.CreateRefreshToken;
using AutoMapper;
using MediatR;

namespace Auction.Application.Features.Tokens.Commands.CreateRefreshToken
{
    public record CreateRefreshTokenCommand : IMapWith<CreateRefreshTokenDto>, IRequest<Result<string>>
    {
        public string Ip { get; set; }
        public Guid UserId { get; set; }
        public bool SkipDeviceLimitCheck { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<CreateRefreshTokenCommand, CreateRefreshTokenDto>()
                .ForMember(command => command.Ip, opt => opt.MapFrom(dto => dto.Ip))
                .ForMember(command => command.UserId, opt => opt.MapFrom(dto => dto.UserId));
        }
    }
}
