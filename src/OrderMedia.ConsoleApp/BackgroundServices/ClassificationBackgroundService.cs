using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OrderMedia.ConsoleApp.Interfaces;
using OrderMedia.ConsoleApp.Orchestrators;

namespace OrderMedia.ConsoleApp.BackgroundServices;

public class ClassificationBackgroundService : BackgroundService
{
    private readonly IOrchestrator _orchestrator;

    public ClassificationBackgroundService([FromKeyedServices(ClassificationOrchestrator.ServiceName)]IOrchestrator orchestrator)
    {
        _orchestrator = orchestrator;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await _orchestrator.RunAsync(stoppingToken);
    }
}