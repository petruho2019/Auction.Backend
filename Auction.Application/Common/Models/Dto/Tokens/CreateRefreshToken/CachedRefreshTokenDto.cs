using Auction.Application.Common.Mappings;
using Auction.Domain.Models;
using AutoMapper;

namespace Auction.Application.Common.Models.Dto.Tokens.CreateRefreshToken
{
    public class CachedRefreshTokenDto : IMapWith<RefreshToken>
    {
        public string Token { get; set; }
        public string CreatedByIp { get; set; }
        public DateTime Expires { get; set; }
        public bool IsExpire => DateTime.Now >= Expires;
        public DateTime Created { get; set; }
        public DateTime? Revoked { get; set; }
        public bool IsActive => Revoked == null && !IsExpire;
        public Guid OwnerId { get; set; }
        public string OwnerUsername { get; set; }
        public string OwnerEmail { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<RefreshToken, CachedRefreshTokenDto>()
                .ForMember(dest => dest.OwnerEmail, opt => opt.MapFrom(src => src.Owner.Email))
                .ForMember(dest => dest.OwnerUsername, opt => opt.MapFrom(src => src.Owner.Username));

        }
    }
}
