using Auction.Application.Common.Mappings;
using Auction.Application.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Auction.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddMediatR(conf => conf.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

            services.AddAutoMapper(conf
                => conf.AddProfile(new AssemblyMappingProfile(Assembly.GetExecutingAssembly())));

            //services.AddTransient<IJwtProvider>(provider => new JwtProvider.JwtProvider(configuration));

            return services;
        }
    }
}
