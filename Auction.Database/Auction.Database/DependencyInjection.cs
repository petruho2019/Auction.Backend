using Auction.Application.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Auction.Database
{
    public static class DependencyInjection 
    {
        public static IServiceCollection AddAuctionContext(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = 
                configuration["ConnectionString"] ?? throw new ArgumentNullException(nameof(configuration), "ConnectionString is missing in configuration.");

            services.AddDbContext<AuctionContext>(opt =>
            {
                opt.UseNpgsql(connectionString, options => options.MigrationsAssembly("Auction.WebApi"));
            });

            services.AddScoped<IAuctionContext>(provider
                => provider.GetRequiredService<AuctionContext>());

            return services;
        }
    }
}
