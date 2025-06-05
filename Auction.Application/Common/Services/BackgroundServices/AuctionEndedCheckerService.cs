using Auction.Application.Features.Auctions.Commands.EndAction;
using Auction.Application.Features.Notifications;
using Auction.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Auction.Application.Common.Services.BackgroundServices
{
    public class AuctionEndedCheckerService(IServiceProvider services, ILogger<AuctionEndedCheckerService> logger) : BackgroundService
    {

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await CheckEndedAuction();
                await Task.Delay(TimeSpan.FromSeconds(30), stoppingToken);
            }
        }

        private async Task CheckEndedAuction()
        {
            using var scope = services.CreateScope();

            var dbContext = scope.ServiceProvider.GetRequiredService<IAuctionContext>();
            var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

            var endedAuctions = await dbContext.Auctions
                .Where(a => a.End <= DateTime.Now && !a.IsEnded)
                .ToListAsync();

            foreach (var auction in endedAuctions)
            {
                try
                {
                    await mediator.Send(new CompleteAuctionCommand() { Auction = auction });
                    await mediator.Send(new SendMailNotification() { Auction = auction });
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, $"Ошибка при завершении аукциона {auction.Id}");
                }
            }
        }
    }
}
