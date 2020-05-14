using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SpaceService.Servers;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SpaceService.Services
{
    public class ConsumeScopedServiceHostedService : IHostedService
    {
        private readonly IServiceProvider services;
        private readonly ILogger<ConsumeScopedServiceHostedService> logger;
        private IServiceScope scope;

        public ConsumeScopedServiceHostedService(IServiceProvider services,
            ILogger<ConsumeScopedServiceHostedService> logger)
        {
            this.services = services;
            this.logger = logger;
        }


        public async Task StartAsync(CancellationToken stoppingToken)
        {
            scope = services.CreateScope();
            var scopedProcessingService =
                scope.ServiceProvider
                        .GetRequiredService<SpaceServiceServer>();

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