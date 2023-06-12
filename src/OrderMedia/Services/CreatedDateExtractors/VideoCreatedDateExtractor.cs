using System;
using MetadataExtractor;
using MetadataExtractor.Formats.QuickTime;
using System.Collections.Generic;
using System.Globalization;
using OrderMedia.Interfaces;
using System.Linq;

namespace OrderMedia.Services.CreatedDateExtractors
{
    /// <summary>
    /// Date Extractor for media type Video.
    /// </summary>
	public class VideoCreatedDateExtractor : ICreatedDateExtractor
    {
        private readonly IMetadataExtractorService _metadataExtractor;

        public VideoCreatedDateExtractor(IMetadataExtractorService metadataExtractor)
        {
            _metadataExtractor = metadataExtractor;
        }

        public DateTime GetCreatedDateTime(string mediaPath)
        {
            var metadataDateTime = GetDateFromMetadata(mediaPath);

            return SetCreatedDateTimeFromMetadataString(metadataDateTime);
        }

        private string GetDateFromMetadata(string mediaPath)
        {
            var directories = _metadataExtractor.GetImageDirectories(mediaPath);

            var quickTimeDirectory = directories.OfType<QuickTimeMetadataHeaderDirectory>().FirstOrDefault();
            var videoCreationDate = quickTimeDirectory?.GetDescription(QuickTimeMetadataHeaderDirectory.TagCreationDate);

            return videoCreationDate;
        }

        private DateTime SetCreatedDateTimeFromMetadataString(string metadataString)
        {
            DateTime.TryParseExact(metadataString, "ddd MMM dd HH:mm:ss zzz yyyy", new CultureInfo("en-UK", false), System.Globalization.DateTimeStyles.None, out DateTime videoDate);

            return videoDate;
        }
    }
}

