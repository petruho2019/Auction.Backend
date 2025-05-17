using Auction.Application.Features.Auctions.Commands.EndAction;
using Auction.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auction.Application.Common.Services.BackgroundServices
{
    public class AuctionEndedCheckerService : BackgroundService
    {
        private readonly IServiceProvider _services;
        private readonly ILogger<AuctionEndedCheckerService> _logger;

        public AuctionEndedCheckerService(IServiceProvider services, ILogger<AuctionEndedCheckerService> logger)
        {
            _services = services;
            _logger = logger;
        }
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
            using var scope = _services.CreateScope();

            var dbContext = scope.ServiceProvider.GetRequiredService<IAuctionContext>();
            var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

            var endedAuctions = await dbContext.Auctions
                .Where(a => a.DateEnd <= DateTime.Now && !a.IsEnded)
                .ToListAsync();

            foreach (var auction in endedAuctions)
            {
                try
                {
                    await mediator.Send(new CompleteAuctionCommand() { AuctionId = auction.Id });
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"Ошибка при завершении аукциона {auction.Id}");
                }
            }
        }
    }
}
