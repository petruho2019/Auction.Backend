namespace Auction.Application.Interfaces
{
    public interface ICurrentUserService
    {
        string Username { get; }
        Guid UserId { get; }
    }
}
