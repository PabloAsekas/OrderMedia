using System;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;
using OrderMedia.Interfaces;

namespace OrderMedia.Services.CreatedDateExtractors
{
    /// <summary>
    /// Date Extractor for media type WhatsApp.
    /// </summary>
	public class WhatsAppCreatedDateExtractor : ICreatedDateExtractor
    {
        public DateTime GetCreatedDateTime(string mediaPath)
        {
            string name = GetName(mediaPath);

            string pattern = @"[0-9]{4}-(0[1-9]|1[0-2])-(0[1-9]|[1-2][0-9]|3[0-1])-(0[0-9]|[1-2][0-9])-([0-5][0-9])-([0-5][0-9])";

            Match m = Regex.Match(name, pattern, RegexOptions.IgnoreCase);

            // We assume that the regex will succeed.

            return GetCreatedDateTimeFromMetadataString(m.Value);
        }

        private string GetName(string mediaPath)
        {
            return Path.GetFileNameWithoutExtension(mediaPath);
        }

        private DateTime GetCreatedDateTimeFromMetadataString(string metadataString)
        {
            DateTime.TryParseExact(metadataString, "yyyy-MM-dd-HH-mm-ss", new CultureInfo("es-ES", false), System.Globalization.DateTimeStyles.None, out DateTime imageDate);

            return imageDate;
        }
    }
}

