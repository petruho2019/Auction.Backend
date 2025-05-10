using Auction.Application.Common.Mappings;
using Auction.Application.Interfaces;
using Auction.Application.JwtProvider;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

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

            services.AddTransient<IJwtProvider>(provider => new JwtProvider.JwtProvider(configuration));

            return services;
        }
    }
}
