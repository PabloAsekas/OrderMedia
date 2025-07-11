using OrderMedia.Interfaces;
using OrderMedia.Interfaces.Factories;
using OrderMedia.Interfaces.Handlers;
using OrderMedia.Models;

namespace OrderMedia.Services;
public class MetadataExtractorService : IMetadataExtractorService
{
    private readonly ICreatedDateHandler _createdDateHandlerChain;
    
    public MetadataExtractorService(ICreatedDateChainFactory createdDateChainFactory)
    {
        _createdDateHandlerChain = createdDateChainFactory.CreateChain();
    }
    
    public CreatedDateInfo? GetCreatedDate(string mediaPath)
    {
        return _createdDateHandlerChain.GetCreatedDateInfo(mediaPath);
    }
}
