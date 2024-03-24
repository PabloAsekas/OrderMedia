using System;
using System.Globalization;
using OrderMedia.Interfaces;

namespace OrderMedia.Services.CreatedDateExtractors
{
    /// <summary>
    /// Date Extractor for media type Raw.
    /// </summary>
	public class RawCreatedDateExtractor : BaseCreatedDateExtractor
    {
        private readonly IMetadataExtractorService _metadataExtractor;
        private readonly IIOService _ioService;
        private readonly IXmpExtractorService _xmpExtractorService;

        public RawCreatedDateExtractor(IMetadataExtractorService metadataExtractor, IIOService ioService, IXmpExtractorService xmpExtractorService)
        {
            _metadataExtractor = metadataExtractor;
            _ioService = ioService;
            _xmpExtractorService = xmpExtractorService;
        }

        public override DateTime GetCreatedDateTime(string mediaPath)
        {
            var xmpFilePath = GetXmpFilePath(mediaPath);

            string dateTimeAsString;
            string format;

            if (_ioService.FileExists(xmpFilePath))
            {
                dateTimeAsString = _xmpExtractorService.GetCreatedDate(xmpFilePath);
                // we assume that the date will come with the format yyyy-MM-ddTHH:mm:ss
                format = "yyyy-MM-ddTHH:mm:ss";
            }
            else
            {
                dateTimeAsString = _metadataExtractor.GetRawCreatedDate(mediaPath);
                format = "yyyy:MM:dd HH:mm:ss";
            }

            return GetDateTimeFromStringWithFormat(dateTimeAsString, format, new CultureInfo("es-ES", false));
        }

        private string GetXmpFilePath(string mediaPath)
        {
            var folder = _ioService.GetDirectoryName(mediaPath);
            var nameWithoutExtension = _ioService.GetFileNameWithoutExtension(mediaPath);

            return _ioService.Combine(new string[] { folder, $"{nameWithoutExtension}.xmp" });
        }
    }
}