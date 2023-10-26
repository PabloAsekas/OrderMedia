using System;
using System.Globalization;
using OrderMedia.Interfaces;

namespace OrderMedia.Services.CreatedDateExtractors
{
    /// <summary>
    /// Date Extractor for media type Image.
    /// </summary>
	public class ImageCreatedDateExtractor : BaseCreatedDateExtractor
    {
        private readonly IMetadataExtractorService _metadataExtractor;

        public ImageCreatedDateExtractor(IMetadataExtractorService metadataExtractor)
        {
            _metadataExtractor = metadataExtractor;
        }

        public override DateTime GetCreatedDateTime(string mediaPath)
        {
            var metadataDateTime = _metadataExtractor.GetImageCreatedDate(mediaPath);

            return GetDateTimeFromStringWithFormat(metadataDateTime, "yyyy:MM:dd HH:mm:ss", new CultureInfo("es-ES", false));
        }
    }
}

