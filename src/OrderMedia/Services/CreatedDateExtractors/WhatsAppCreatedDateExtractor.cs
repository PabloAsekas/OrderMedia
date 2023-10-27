using System;
using System.Globalization;
using System.Text.RegularExpressions;
using OrderMedia.Interfaces;

namespace OrderMedia.Services.CreatedDateExtractors
{
    /// <summary>
    /// Date Extractor for media type WhatsApp.
    /// </summary>
	public class WhatsAppCreatedDateExtractor : BaseCreatedDateExtractor
    {
        private readonly IIOService _ioService;

        public WhatsAppCreatedDateExtractor(IIOService ioService)
        {
            _ioService = ioService;
        }

        public override DateTime GetCreatedDateTime(string mediaPath)
        {
            string name = _ioService.GetFileNameWithoutExtension(mediaPath);

            string pattern = @"[0-9]{4}-(0[1-9]|1[0-2])-(0[1-9]|[1-2][0-9]|3[0-1])-(0[0-9]|[1-2][0-9])-([0-5][0-9])-([0-5][0-9])";

            Match m = Regex.Match(name, pattern, RegexOptions.IgnoreCase);

            // We assume that the regex will succeed.

            return GetDateTimeFromStringWithFormat(m.Value, "yyyy-MM-dd-HH-mm-ss", new CultureInfo("es-ES", false));
        }
    }
}

