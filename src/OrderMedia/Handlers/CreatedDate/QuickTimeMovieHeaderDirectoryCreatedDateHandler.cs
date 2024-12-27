using MetadataExtractor.Formats.QuickTime;
using OrderMedia.Interfaces;
using OrderMedia.Models;

namespace OrderMedia.Handlers.CreatedDate;

public class QuickTimeMovieHeaderDirectoryCreatedDateHandler: BaseCreatedDateHandler
{
    private readonly IImageMetadataReader _imageMetadataReader;

    public QuickTimeMovieHeaderDirectoryCreatedDateHandler(IImageMetadataReader imageMetadataReader)
    {
        _imageMetadataReader = imageMetadataReader;
    }
        
    public override CreatedDateInfo? GetCreatedDateInfo(string mediaPath)
    {
        var createdDate = _imageMetadataReader.GetMetadataByDirectoryTypeAndTag<QuickTimeMovieHeaderDirectory>(mediaPath, QuickTimeMovieHeaderDirectory.TagCreated);

        var createdDateInfo = CreateCreatedDateInfo(createdDate, "ddd MMM dd HH:mm:ss yyyy");
            
        return createdDateInfo ?? base.GetCreatedDateInfo(mediaPath);
    }
}