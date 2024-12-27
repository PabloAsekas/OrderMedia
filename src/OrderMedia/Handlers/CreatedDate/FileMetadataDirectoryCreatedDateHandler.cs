using MetadataExtractor.Formats.FileSystem;
using OrderMedia.Interfaces;
using OrderMedia.Models;

namespace OrderMedia.Handlers.CreatedDate;

public class FileMetadataDirectoryCreatedDateHandler : BaseCreatedDateHandler
{
    private readonly IImageMetadataReader _imageMetadataReader;

    public FileMetadataDirectoryCreatedDateHandler(IImageMetadataReader imageMetadataReader)
    {
        _imageMetadataReader = imageMetadataReader;
    }

    public override CreatedDateInfo? GetCreatedDateInfo(string mediaPath)
    {
        var createdDate = _imageMetadataReader.GetMetadataByDirectoryTypeAndTag<FileMetadataDirectory>(mediaPath, FileMetadataDirectory.TagFileModifiedDate);

        var createdDateInfo = CreateCreatedDateInfo(createdDate, "ddd MMM dd HH:mm:ss zzz yyyy");
        
        return createdDateInfo ?? base.GetCreatedDateInfo(mediaPath);
    }
}