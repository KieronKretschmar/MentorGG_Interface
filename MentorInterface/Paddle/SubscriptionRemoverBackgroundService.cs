using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MentorInterface.Paddle
{
    /// <summary>
    /// Can run as a BackgroundService and regularly removes all expired subscriptions.
    /// </summary>
    public class SubscriptionRemoverBackgroundService : BackgroundService
    {
        private readonly ILogger<SubscriptionRemoverBackgroundService> _logger;
        private readonly IServiceScopeFactory _scopeFactory;

        /// <summary>
        /// Time between cycles of removing all expired subscriptions, in seconds.
        /// </summary>
        private const int UPDATE_INTERVAL_SECONDS = 5 * 60;

        public SubscriptionRemoverBackgroundService(ILogger<SubscriptionRemoverBackgroundService> logger, IServiceScopeFactory scopeFactory)
        {
            _logger = logger;
            _scopeFactory = scopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            _logger.LogDebug($"SubscriptionRemoverBackgroundService is starting.");

            cancellationToken.Register(() =>
                _logger.LogDebug($"SubscriptionRemoverBackgroundService is stopping."));

            while (!cancellationToken.IsCancellationRequested)
            {
                // Create a scope so we can use a (transient) SubscriptionRemover
                using (var scope = _scopeFactory.CreateScope())
                {
                    _logger.LogDebug($"SubscriptionRemoverBackgroundService is doing background work.");
                    try
                    {
                        var remover = scope.ServiceProvider.GetRequiredService<SubscriptionRemover>();
                        await remover.RemoveAllExpiredSubscriptionsAsync();
                    }
                    catch (Exception e)
                    {
                        _logger.LogError("Error when removing expired subscriptions. This can happen once when the database is not yet migrated.", e);
                    }
                    await Task.Delay(UPDATE_INTERVAL_SECONDS * 1000, cancellationToken);
                }
            }

            _logger.LogDebug($"SubscriptionRemoverBackgroundService is stopping.");
            await Task.CompletedTask;
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await Task.CompletedTask;
        }
    }
}
