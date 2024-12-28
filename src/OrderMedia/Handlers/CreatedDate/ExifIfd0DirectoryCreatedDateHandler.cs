using MetadataExtractor.Formats.Exif;
using OrderMedia.Interfaces;
using OrderMedia.Models;

namespace OrderMedia.Handlers.CreatedDate;

public class ExifIfd0DirectoryCreatedDateHandler : BaseCreatedDateHandler
{
    private readonly IImageMetadataReader _imageMetadataReader;

    public ExifIfd0DirectoryCreatedDateHandler(IImageMetadataReader imageMetadataReader)
    {
        _imageMetadataReader = imageMetadataReader;
    }

    public override CreatedDateInfo? GetCreatedDateInfo(string mediaPath)
    {
        var createdDate = _imageMetadataReader.GetMetadataByDirectoryTypeAndTag<ExifIfd0Directory>(mediaPath, ExifIfd0Directory.TagDateTime);

        var createdDateInfo = CreateCreatedDateInfo(createdDate, "yyyy:MM:dd HH:mm:ss");
        
        return createdDateInfo ?? base.GetCreatedDateInfo(mediaPath);
    }
}