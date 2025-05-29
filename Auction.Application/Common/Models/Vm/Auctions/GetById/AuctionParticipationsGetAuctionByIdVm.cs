using Auction.Application.Common.Mappings;
using AutoMapper;

namespace Auction.Application.Common.Models.Vm.Auctions.GetById
{
    public class AuctionParticipationsGetAuctionByIdVm : IMapWith<Domain.Models.AuctionParticipation>
    {
        public string UsernameBid { get; set; }
        public double Bid { get; set; }
        public DateTime DateBid { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Domain.Models.AuctionParticipation, AuctionParticipationsGetAuctionByIdVm>()
                .ForMember(dest => dest.UsernameBid, opt => opt.MapFrom(src => src.User.Username))
                .ForMember(dest => dest.Bid, opt => opt.MapFrom(src => src.BidPrice))
                .ForMember(dest => dest.DateBid, opt => opt.MapFrom(src => src.BidTime));
        }
    }

}
