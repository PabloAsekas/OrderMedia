using System.Linq;
using MetadataExtractor;
using MetadataExtractor.Formats.Xmp;
using OrderMedia.Models;

namespace OrderMedia.Handlers.CreatedDate;

public class XmpDirectoryCreatedDateHandler : BaseCreatedDateHandler
{
    public override CreatedDateInfo? GetCreatedDateInfo(string mediaPath)
    {
        if (!mediaPath.EndsWith(".dng"))
        {
            return base.GetCreatedDateInfo(mediaPath);
        }
        
        var directories = ImageMetadataReader.ReadMetadata(mediaPath);
            
        var xmpDirectory = directories.OfType<XmpDirectory>().FirstOrDefault();
        
        if (xmpDirectory == null)
            return base.GetCreatedDateInfo(mediaPath);
        
        var properties = xmpDirectory.GetXmpProperties();

        properties.TryGetValue("xmp:CreateDate", out var createdDate);

        createdDate = createdDate?.Substring(0, 19);

        var createdDateInfo = CreateCreatedDateInfo(createdDate, "yyyy-MM-ddTHH:mm:ss");
        
        return createdDateInfo ?? base.GetCreatedDateInfo(mediaPath);
    }
}