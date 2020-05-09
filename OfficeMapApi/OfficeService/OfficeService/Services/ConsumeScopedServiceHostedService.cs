using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using OfficeService.Messaging.RabbitMQ;
using System;
using System.Threading;
using System.Threading.Tasks;

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
                    .GetRequiredService<OfficeServiceServer>();

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