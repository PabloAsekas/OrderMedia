using OrderMedia.Interfaces;
using OrderMedia.Models;

namespace OrderMedia.Handlers.CreatedDate;

public class XmpCreatedDateHandler : BaseCreatedDateHandler
{
    private readonly IIoWrapper _ioWrapper;
    private readonly IXmpExtractorService _xmpExtractorService;

    public XmpCreatedDateHandler(IIoWrapper ioWrapper, IXmpExtractorService xmpExtractorService)
    {
        _ioWrapper = ioWrapper;
        _xmpExtractorService = xmpExtractorService;
    }

    public override CreatedDateInfo? GetCreatedDateInfo(string mediaPath)
    {
        var xmpFilePath = GetXmpFilePath(mediaPath);

        var createdDate = _xmpExtractorService.GetCreatedDate(xmpFilePath);

        var createdDateInfo = CreateCreatedDateInfo(createdDate, "yyyy-MM-ddTHH:mm:ss");
        
        return createdDateInfo ?? base.GetCreatedDateInfo(mediaPath);
    }
    
    private string GetXmpFilePath(string mediaPath)
    {
        var folder = _ioWrapper.GetDirectoryName(mediaPath);
        var nameWithoutExtension = _ioWrapper.GetFileNameWithoutExtension(mediaPath);

        return _ioWrapper.Combine(new[] { folder, $"{nameWithoutExtension}.xmp" });
    }
}
