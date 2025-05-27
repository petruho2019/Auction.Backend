using Auction.Application.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Auction.JwtProvider
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddJwtProvider(this IServiceCollection services,
            IConfiguration configuration)
        {

            services.AddTransient<IJwtProvider, JwtProvider>();

            return services;
        }
    }
}
