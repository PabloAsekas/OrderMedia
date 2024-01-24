using System;
using System.Globalization;
using OrderMedia.Interfaces;

namespace OrderMedia.Services.CreatedDateExtractors
{
    /// <summary>
    /// Date Extractor for media type Video.
    /// </summary>
	public class VideoCreatedDateExtractor : BaseCreatedDateExtractor
    {
        private readonly IMetadataExtractorService _metadataExtractor;

        public VideoCreatedDateExtractor(IMetadataExtractorService metadataExtractor)
        {
            _metadataExtractor = metadataExtractor;
        }

        public override DateTime GetCreatedDateTime(string mediaPath)
        {
            var metadataDateTime = _metadataExtractor.GetVideoCreatedDate(mediaPath);

            //return GetDateTimeFromStringWithFormat(metadataDateTime, "ddd MMM dd HH:mm:ss zzz yyyy", new CultureInfo("en-UK", false));
            return GetDateTimeFromStringWithFormat(metadataDateTime, "ddd MMM dd HH:mm:ss zzz yyyy", new CultureInfo("es-ES", false));
        }
    }
}

