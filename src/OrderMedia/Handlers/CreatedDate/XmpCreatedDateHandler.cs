using OrderMedia.Interfaces;
using OrderMedia.Models;

namespace OrderMedia.Handlers.CreatedDate;

public class XmpCreatedDateHandler : BaseCreatedDateHandler
{
    private readonly IIOService _ioService;
    private readonly IXmpExtractorService _xmpExtractorService;

    public XmpCreatedDateHandler(IIOService ioService, IXmpExtractorService xmpExtractorService)
    {
        _ioService = ioService;
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
        var folder = _ioService.GetDirectoryName(mediaPath);
        var nameWithoutExtension = _ioService.GetFileNameWithoutExtension(mediaPath);

        return _ioService.Combine(new[] { folder, $"{nameWithoutExtension}.xmp" });
    }
}