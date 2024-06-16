using OrderMedia.Interfaces;
using OrderMedia.Models;
using SixLabors.ImageSharp.Metadata.Profiles.Exif;

namespace OrderMedia.Handlers.Processor;

public class CreatedDateAggregatorProcessorHandler : BaseProcessorHandler
{
    private readonly IMetadataAggregatorService _metadataAggregatorService;

    public CreatedDateAggregatorProcessorHandler(IMetadataAggregatorService metadataAggregatorService)
    {
        _metadataAggregatorService = metadataAggregatorService;
    }

    public override void Process(Media media)
    {
        using var image = _metadataAggregatorService.GetImage(media.NewMediaPath);
        
        var exifProfile = image.Metadata.ExifProfile ?? new ExifProfile();

        var mediaDateTime = media.CreatedDateTime.ToString("yyyy:MM:dd HH:mm:ss");
        
        exifProfile.SetValue(ExifTag.DateTimeOriginal, mediaDateTime);
        exifProfile.SetValue(ExifTag.DateTime, mediaDateTime);
        exifProfile.SetValue(ExifTag.DateTimeDigitized, mediaDateTime);
        exifProfile.SetValue(ExifTag.OffsetTime, "+01:00");
        exifProfile.SetValue(ExifTag.OffsetTimeOriginal, "+01:00");
        exifProfile.SetValue(ExifTag.OffsetTimeDigitized, "+01:00");
        
        image.Metadata.ExifProfile = exifProfile;
        
        _metadataAggregatorService.SaveImage(image, media.NewMediaPath);
        
        base.Process(media);
    }
}