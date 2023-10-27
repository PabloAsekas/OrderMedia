using System;
using System.Collections.Generic;
using System.Linq;
using MetadataExtractor;
using MetadataExtractor.Formats.Exif;
using MetadataExtractor.Formats.QuickTime;
using OrderMedia.Interfaces;

namespace OrderMedia.Services
{
    public class MetadataExtractorService : IMetadataExtractorService
	{
        public IEnumerable<Directory> GetImageDirectories(string mediaPath)
        {
            return ImageMetadataReader.ReadMetadata(mediaPath);
        }

        private string GetCreatedDate<T>(string mediaPath, int tagType)
        {
            var directories = GetImageDirectories(mediaPath);
            var dateDirectory = directories.OfType<T>().FirstOrDefault() as Directory;

            return dateDirectory?.GetDescription(tagType);
        }

        public string GetImageCreatedDate(string mediaPath)
        {
            return GetCreatedDate< ExifSubIfdDirectory>(mediaPath, ExifDirectoryBase.TagDateTimeOriginal);
        }

        public string GetRawCreatedDate(string mediaPath)
        {
            return GetCreatedDate<ExifIfd0Directory>(mediaPath, ExifIfd0Directory.TagDateTime);
        }

        public string GetVideoCreatedDate(string mediaPath)
        {
            return GetCreatedDate<QuickTimeMetadataHeaderDirectory>(mediaPath, QuickTimeMetadataHeaderDirectory.TagCreationDate);
        }
    }
}

