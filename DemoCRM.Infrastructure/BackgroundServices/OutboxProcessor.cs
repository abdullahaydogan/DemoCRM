using DemoCRM.Persistance.Context;
using DemoCRM.Infrastructure.Messaging;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace DemoCRM.Infrastructure.BackgroundServices
{
    public class OutboxProcessor : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;

        public OutboxProcessor(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using var scope = _scopeFactory.CreateScope();

                var context = scope.ServiceProvider.GetRequiredService<CrmContext>();
                var publisher = scope.ServiceProvider.GetRequiredService<RabbitMqPublisher>();

                var messages = await context.OutboxMessages
                    .Where(x => !x.IsProcessed)
                    .OrderBy(x => x.CreatedDate)
                    .Take(20)
                    .ToListAsync(stoppingToken);

                foreach (var message in messages)
                {
                    try
                    {
                        await publisher.PublishAsync(message.EventType, message.Payload);

                        message.IsProcessed = true;
                        message.ProcessedDate = DateTime.UtcNow;
                    }
                    catch(Exception ex)
                    {
                        message.RetryCount++;
                        message.LastError = ex.Message;
                        if (message.RetryCount >= 5)
                        {
                            message.IsProcessed = true;
                            message.ProcessedDate = DateTime.UtcNow;
                        }
                    }
                }

                await context.SaveChangesAsync(stoppingToken);

                await Task.Delay(3000, stoppingToken);
            }
        }
    }
}