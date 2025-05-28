using Auction.Application.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

namespace Auction.CacheService
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddCache
            (this IServiceCollection services,
            IConfiguration configuration)
        {
            var multiplexer = ConnectionMultiplexer.Connect(configuration["Redis:Configuration"] 
                ?? throw new ArgumentNullException("Configuration for redis not found!")
                );

            if (!multiplexer.IsConnected)
                throw new InvalidOperationException("Не удалось подлючиться к redis");

            services.AddSingleton<IConnectionMultiplexer>(multiplexer);
            services.AddScoped<ICacheService, CacheService>();

            return services;
        }
    }
}
