using OrderMedia.Interfaces;
using OrderMedia.Interfaces.Factories;
using OrderMedia.Models;

namespace OrderMedia.Services;

/// <summary>
/// Classification service.
/// </summary>
public class ClassificationService : IClassificationService
{
    private readonly IProcessorChainFactory _processorChainFactory;

    public ClassificationService(IProcessorChainFactory processorChainFactory)
    {
        _processorChainFactory = processorChainFactory;
    }

    public void Process(Media media)
    {
        var processor = _processorChainFactory.Build(media.MediaType);

        processor?.Process(media);
    }
}
