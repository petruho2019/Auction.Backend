using Auction.Application.Common.Mappings;
using AutoMapper;

namespace Auction.Application.Common.Models.Vm.AuctionParticipation.Create
{
    public class AuctionParticipationMakeABigVm : IMapWith<Domain.Models.AuctionParticipation>
    {
        public double BidPrice { get; set; }
        public UserBidVm BidUser { get; set; }
        public DateTime BidTime { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Domain.Models.AuctionParticipation, AuctionParticipationMakeABigVm>()
                .ForMember(vm => vm.BidPrice, opt => opt.MapFrom(ap => ap.BidPrice))
                .ForPath(vm => vm.BidUser.Username, opt => opt.MapFrom(ap => ap.User.Username))
                .ForPath(vm => vm.BidUser.Email, opt => opt.MapFrom(ap => ap.User.Email))
                .ForMember(vm => vm.BidTime, opt => opt.MapFrom(ap => ap.BidTime));
        }
    }
}
