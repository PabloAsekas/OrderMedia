using System;
using MetadataExtractor;
using MetadataExtractor.Formats.Exif;
using System.Collections.Generic;
using System.Globalization;
using OrderMedia.Interfaces;
using System.Linq;
using OrderMedia.Models;

namespace OrderMedia.Services.CreatedDateExtractors
{
    /// <summary>
    /// Date Extractor for media type Raw.
    /// </summary>
	public class RawCreatedDateExtractor : ICreatedDateExtractor
    {
        private readonly IMetadataExtractorService _metadataExtractor;

        public RawCreatedDateExtractor(IMetadataExtractorService metadataExtractor)
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

            var exifDirectory = directories.OfType<ExifIfd0Directory>().FirstOrDefault();
            var imageCreationDate = exifDirectory?.GetDescription(ExifIfd0Directory.TagDateTime);

            return imageCreationDate;
        }

        private DateTime GetCreatedDateTimeFromMetadataString(string metadataString)
        {
            DateTime.TryParseExact(metadataString, "yyyy:MM:dd HH:mm:ss", new CultureInfo("es-ES", false), System.Globalization.DateTimeStyles.None, out DateTime imageDate);

            return imageDate;
        }
    }
}