using System;
using System.Globalization;
using OrderMedia.Interfaces;

namespace OrderMedia.Services;

public class CreatedDateExtractorService : ICreatedDateExtractorService
{
    private readonly IMetadataExtractorService _metadataExtractor;

    public CreatedDateExtractorService(IMetadataExtractorService metadataExtractor)
    {
        _metadataExtractor = metadataExtractor;
    }
    
    public DateTimeOffset GetCreatedDateTimeOffset(string mediaPath)
    {
        var createdDateInfo = _metadataExtractor.GetCreatedDate(mediaPath);

        return createdDateInfo is null ? default : GetDateTimeFromStringWithFormat(createdDateInfo.CreatedDate, createdDateInfo.Format, CultureInfo.CurrentCulture);
    }
    
    private static DateTimeOffset GetDateTimeFromStringWithFormat(string metadataString, string format, CultureInfo cultureInfo)
    {
        DateTimeOffset.TryParseExact(metadataString, format, cultureInfo, DateTimeStyles.None, out DateTimeOffset imageDate);
        
        return imageDate;
    }
}