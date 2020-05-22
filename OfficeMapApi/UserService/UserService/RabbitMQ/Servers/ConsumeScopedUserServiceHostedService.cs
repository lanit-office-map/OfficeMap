using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace UserService.RabbitMQ.Servers
{
    public class ConsumeScopedUserServiceHostedService : IHostedService
    {
        private readonly IServiceProvider services;
        private readonly ILogger<ConsumeScopedUserServiceHostedService> logger;
        private IServiceScope scope;

        public ConsumeScopedUserServiceHostedService(IServiceProvider services,
            ILogger<ConsumeScopedUserServiceHostedService> logger)
        {
            this.services = services;
            this.logger = logger;
        }


        public async Task StartAsync(CancellationToken stoppingToken)
        {
            scope = services.CreateScope();
            var scopedProcessingService =
                scope.ServiceProvider
                        .GetRequiredService<UserServiceServer>();

            await Task.CompletedTask;
        }

        public async Task StopAsync(CancellationToken stoppingToken)
        {
            scope.Dispose();

            logger.LogInformation(
                "Consume Scoped Service Hosted Service is stopping.");

            await Task.CompletedTask;
        }
    }
}