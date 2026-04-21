using System;
using System.Collections.Generic;
using System.Linq;
using MetadataExtractor;
using MetadataExtractor.Formats.Xmp;
using Microsoft.Extensions.Logging;
using OrderMedia.Extensions;
using OrderMedia.Interfaces;

namespace OrderMedia.Wrappers;

public class ImageMetadataReaderWrapper : IImageMetadataReader
{
    private readonly ILogger<IImageMetadataReader> _logger;

    public ImageMetadataReaderWrapper(ILogger<IImageMetadataReader> logger)
    {
        _logger = logger;
    }

    public string GetMetadataByDirectoryTypeAndTag<T>(string mediaPath, int tagType)
    {
        try
        {
            var directories = ReadMetadata(mediaPath);
            var dateDirectory = directories.OfType<T>().FirstOrDefault() as Directory;
            
            return dateDirectory?.GetDescription(tagType);

        }
        catch (Exception e)
        {
            _logger.ErrorReadingMetadata(mediaPath);
        }

        return null;
    }

    public string GetMetadataFromXmpDirectory(string mediaPath, string propertyName)
    {
        try
        {
            var directories = ReadMetadata(mediaPath);
            var xmpDirectory = directories.OfType<XmpDirectory>().FirstOrDefault();

            if (xmpDirectory == null)
            {
                return null;
            }

            var properties = xmpDirectory.GetXmpProperties();

            properties.TryGetValue(propertyName, out var propertyValue);

            return propertyValue;
        }
        catch (Exception e)
        {
            _logger.ErrorReadingMetadata(mediaPath);
        }

        return null;
    }
    
    private static IReadOnlyList<Directory> ReadMetadata(string mediaPath)
    {
        return ImageMetadataReader.ReadMetadata(mediaPath);
    }
}