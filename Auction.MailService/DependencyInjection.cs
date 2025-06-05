using Auction.Application.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Auction.MailService
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddMailService(this IServiceCollection services)
        {

            services.AddTransient<IMailService, MailService>();

            return services;
        }
    }
}