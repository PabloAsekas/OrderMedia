using System;
using OrderMedia.Interfaces.Factories;
using OrderMedia.Interfaces.Handlers;

namespace OrderMedia.Factories;

/// <summary>
/// Processor Handler factory. To create <see cref="IProcessorHandler"/>.
/// </summary>
public class ProcessorHandlerFactory : IProcessorHandlerFactory
{
    private Func<IServiceProvider, IProcessorHandler> Factory { get; }

    public ProcessorHandlerFactory(Func<IServiceProvider, IProcessorHandler> factory)
    {
        Factory = factory;
    }

    public IProcessorHandler CreateInstance(IServiceProvider serviceProvider)
        => Factory(serviceProvider);
}
