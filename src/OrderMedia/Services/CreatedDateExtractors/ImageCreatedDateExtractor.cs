using System;
using MetadataExtractor.Formats.Exif;
using System.Globalization;
using OrderMedia.Interfaces;
using System.Linq;

namespace OrderMedia.Services.CreatedDateExtractors
{
    /// <summary>
    /// Date Extractor for media type Image.
    /// </summary>
	public class ImageCreatedDateExtractor : ICreatedDateExtractor
    {
        private readonly IMetadataExtractorService _metadataExtractor;

        public ImageCreatedDateExtractor(IMetadataExtractorService metadataExtractor)
        {
            _metadataExtractor = metadataExtractor;
        }

        public DateTime GetCreatedDateTime(string mediaPath)
        {
            var metadataDateTime = GetDateFromMetadata(mediaPath);

            return GetCreatedDateTimeFromMetadataString(metadataDateTime);
        }

        private string GetDateFromMetadata(string mediaPath)
        {
            var directories = _metadataExtractor.GetImageDirectories(mediaPath);

            var exifDirectory = directories.OfType<ExifSubIfdDirectory>().FirstOrDefault();
            var imageCreationDate = exifDirectory?.GetDescription(ExifDirectoryBase.TagDateTimeOriginal);

            return imageCreationDate;
        }

        private DateTime GetCreatedDateTimeFromMetadataString(string metadataString)
        {
            DateTime.TryParseExact(metadataString, "yyyy:MM:dd HH:mm:ss", new CultureInfo("es-ES", false), System.Globalization.DateTimeStyles.None, out DateTime imageDate);

            return imageDate;
        }
    }
}

