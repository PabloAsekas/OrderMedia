using MetadataExtractor.Formats.QuickTime;
using OrderMedia.Interfaces;
using OrderMedia.Models;

namespace OrderMedia.Handlers.CreatedDate;

public class QuickTimeMetadataHeaderDirectoryCreatedDateHandler : BaseCreatedDateHandler
{
    private readonly IImageMetadataReader _imageMetadataReader;

    public QuickTimeMetadataHeaderDirectoryCreatedDateHandler(IImageMetadataReader imageMetadataReader)
    {
        _imageMetadataReader = imageMetadataReader;
    }
    
    public override CreatedDateInfo GetCreatedDateInfo(string mediaPath)
    {
        var createdDate = _imageMetadataReader.GetMetadataByDirectoryTypeAndTag<QuickTimeMetadataHeaderDirectory>(mediaPath, QuickTimeMetadataHeaderDirectory.TagCreationDate);

        var createdDateInfo = CreateCreatedDateInfo(createdDate, "ddd MMM dd HH:mm:ss zzz yyyy");
        
        return createdDateInfo ?? base.GetCreatedDateInfo(mediaPath);
    }
}