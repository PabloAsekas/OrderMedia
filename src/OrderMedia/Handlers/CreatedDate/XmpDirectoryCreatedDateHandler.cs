using OrderMedia.Interfaces;
using OrderMedia.Models;

namespace OrderMedia.Handlers.CreatedDate;

public class XmpDirectoryCreatedDateHandler : BaseCreatedDateHandler
{
    private readonly IImageMetadataReader _imageMetadataReader;

    public XmpDirectoryCreatedDateHandler(IImageMetadataReader imageMetadataReader)
    {
        _imageMetadataReader = imageMetadataReader;
    }

    public override CreatedDateInfo? GetCreatedDateInfo(string mediaPath)
    {
        if (!mediaPath.EndsWith(".dng"))
        {
            return base.GetCreatedDateInfo(mediaPath);
        }

        var createdDate = _imageMetadataReader.GetMetadataFromXmpDirectory(mediaPath, "xmp:CreateDate");

        if (createdDate == null)
        {
            return base.GetCreatedDateInfo(mediaPath);
        }
        
        createdDate = createdDate.Substring(0, 19);

        var createdDateInfo = CreateCreatedDateInfo(createdDate, "yyyy-MM-ddTHH:mm:ss");
        
        return createdDateInfo ?? base.GetCreatedDateInfo(mediaPath);
    }
}