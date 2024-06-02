using System.Linq;
using MetadataExtractor;
using OrderMedia.Interfaces;

namespace OrderMedia.Wrappers;

public class ImageMetadataReaderWrapper : IImageMetadataReader
{
    public string GetMetadataByDirectoryTypeAndTag<T>(string mediaPath, int tagType)
    {
        var directories = ImageMetadataReader.ReadMetadata(mediaPath);
        var dateDirectory = directories.OfType<T>().FirstOrDefault() as Directory;

        return dateDirectory?.GetDescription(tagType);
    }
}