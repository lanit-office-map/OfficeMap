﻿using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace WorkplaceService.Servers
{
    public class ConsumeScopedServiceHostedService : IHostedService
    {
        private readonly ILogger<ConsumeScopedServiceHostedService> logger;
        private readonly IServiceProvider services;

        private IServiceScope scope;

        public ConsumeScopedServiceHostedService(
            IServiceProvider services,
            ILogger<ConsumeScopedServiceHostedService> logger)
        {
            this.services = services;
            this.logger = logger;
        }


        public async Task StartAsync(CancellationToken startingToken)
        {
            logger.LogInformation(
                "Consume Scoped Service Hosted Service is starting.");

            scope = services.CreateScope();
            var scopedProcessingService =
                    scope.ServiceProvider
                        .GetRequiredService<WorkplaceServiceServer>();

            await Task.CompletedTask;
        }

        public async Task StopAsync(CancellationToken stoppingToken)
        {
            logger.LogInformation(
                "Consume Scoped Service Hosted Service is stopping.");

            scope.Dispose();

            await Task.CompletedTask;
        }
    }
}
