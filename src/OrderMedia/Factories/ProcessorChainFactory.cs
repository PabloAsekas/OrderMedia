using System;
using System.Collections.Generic;
using Microsoft.Extensions.Options;
using OrderMedia.Configuration;
using OrderMedia.Enums;
using OrderMedia.Interfaces.Factories;
using OrderMedia.Interfaces.Handlers;

namespace OrderMedia.Factories;

/// <summary>
/// Processor chain factory. To create the Processor chain and execute it in the right order. 
/// </summary>
public class ProcessorChainFactory : IProcessorChainFactory
{
    private readonly IServiceProvider _sp;
    private readonly IReadOnlyDictionary<string, IProcessorHandlerFactory> _handlers;
    private readonly ClassificationSettings _classificationSettings;
    
    public ProcessorChainFactory(
        IServiceProvider sp,
        IReadOnlyDictionary<string, IProcessorHandlerFactory> handlers,
        IOptions<ClassificationSettings> options)
    {
        _sp = sp;
        _handlers = handlers;
        _classificationSettings = options.Value;
    }
    
    public IProcessorHandler? Build(MediaType key)
    {
        var processors = _classificationSettings.Processors;
        if (!processors.TryGetValue(key.ToString(), out var names) || names.Count == 0)
            return null;

        IProcessorHandler? first = null;
        IProcessorHandler? current = null;

        foreach (var name in names)
        {
            if (!_handlers.TryGetValue(name, out var processorHandlerFactory))
                throw new InvalidOperationException($"Processor '{name}' not registered");

            var handler = processorHandlerFactory.CreateInstance(_sp);

            if (first == null)
            {
                first = handler;
                current = handler;
            }
            else
            {
                current = current!.SetNext(handler);
            }
        }

        return first;
    }
}