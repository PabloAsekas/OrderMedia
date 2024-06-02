using MetadataExtractor.Formats.Exif;
using OrderMedia.Interfaces;
using OrderMedia.Models;

namespace OrderMedia.Handlers.CreatedDate;

public class ExifSubIfdDirectoryCreatedDateHandler : BaseCreatedDateHandler
{
    private readonly IImageMetadataReader _imageMetadataReader;

    public ExifSubIfdDirectoryCreatedDateHandler(IImageMetadataReader imageMetadataReader)
    {
        _imageMetadataReader = imageMetadataReader;
    }
    
    public override CreatedDateInfo GetCreatedDateInfo(string mediaPath)
    {
        var createdDate = _imageMetadataReader.GetMetadataByDirectoryTypeAndTag<ExifSubIfdDirectory>(mediaPath, ExifDirectoryBase.TagDateTimeOriginal);

        var createdDateInfo = CreateCreatedDateInfo(createdDate, "yyyy:MM:dd HH:mm:ss");
        
        return createdDateInfo ?? base.GetCreatedDateInfo(mediaPath);
    }
}