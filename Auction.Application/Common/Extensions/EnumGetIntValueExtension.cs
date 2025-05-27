using System.Net;

namespace Auction.Application.Common.Extensions
{
    public static class EnumGetIntValueExtension
    {
        public static int GetInt(this HttpStatusCode @enum)
        {
            return (int)@enum;
        }
    }
}
