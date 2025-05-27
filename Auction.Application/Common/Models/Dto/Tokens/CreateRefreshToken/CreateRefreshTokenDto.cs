namespace Auction.Application.Common.Models.Dto.Tokens.CreateRefreshToken
{
    public class CreateRefreshTokenDto
    {
        public string Ip { get; set; }
        public Guid UserId { get; set; }
    }
}
