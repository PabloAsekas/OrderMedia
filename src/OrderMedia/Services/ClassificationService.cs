using OrderMedia.Interfaces;
using OrderMedia.Interfaces.Factories;
using OrderMedia.Models;

namespace OrderMedia.Services;

/// <summary>
/// Classification service.
/// </summary>
public class ClassificationService : IClassificationService
{
    private readonly IProcessorHandlerFactory _processorHandlerFactory;

    public ClassificationService(IProcessorHandlerFactory processorHandlerFactory)
    {
        _processorHandlerFactory = processorHandlerFactory;
    }

    public void Process(Media media)
    {
        var processor = _processorHandlerFactory.CreateProcessorHandler(media.MediaType);

        processor.Process(media);
    }
}
