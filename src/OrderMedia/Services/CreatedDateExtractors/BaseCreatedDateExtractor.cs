using System;
using System.Globalization;
using OrderMedia.Interfaces;

namespace OrderMedia.Services.CreatedDateExtractors
{
    public abstract class BaseCreatedDateExtractor : ICreatedDateExtractor
    {
        public abstract DateTime GetCreatedDateTime(string mediaPath);

        protected static DateTime GetDateTimeFromStringWithFormat(string metadataString, string format, CultureInfo cultureInfo)
        {
            DateTime.TryParseExact(metadataString, format, cultureInfo, DateTimeStyles.None, out DateTime imageDate);

            return imageDate;
        }
    }
}

