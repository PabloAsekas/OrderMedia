using OrderMedia.Interfaces;
using OrderMedia.Models;

namespace OrderMedia.Handlers.CreatedDate;

public class XmpCreatedDateHandler : BaseCreatedDateHandler
{
    private readonly IIOService _ioService;
    private readonly IXmpExtractorService _xmpExtractorService;
    private readonly XmpInfo _xmpInfo;

    public XmpCreatedDateHandler(IIOService ioService, IXmpExtractorService xmpExtractorService, XmpInfo xmpInfo)
    {
        _ioService = ioService;
        _xmpExtractorService = xmpExtractorService;
        _xmpInfo = xmpInfo;
    }

    public override CreatedDateInfo? GetCreatedDateInfo(string mediaPath)
    {
        var xmpFilePath = GetXmpFilePath(mediaPath);

        // EL PROBLEMA ESTÁ EN QUE PUEDE QUE EL FORMATO NO FUNCIONE. ES DECIR, SE RECUPERA UN VALOR DE LA PROPIEDAD, PERO SI EL FORMATO ES EQUIVOCADO, ENTONCES SE DEVUELVE IGUALMENTE.
        var createdDate = _xmpExtractorService.GetValue(xmpFilePath, _xmpInfo.SchemaName, _xmpInfo.PropertyName);

        var createdDateInfo = CreateCreatedDateInfo(createdDate, _xmpInfo.DateFormat);
        
        return createdDateInfo ?? base.GetCreatedDateInfo(mediaPath);
    }
    
    private string GetXmpFilePath(string mediaPath)
    {
        var folder = _ioService.GetDirectoryName(mediaPath);
        var nameWithoutExtension = _ioService.GetFileNameWithoutExtension(mediaPath);

        return _ioService.Combine(new[] { folder, $"{nameWithoutExtension}.xmp" });
    }
}

public class XmpInfo
{
    public string SchemaName { get; set; }
    public string PropertyName { get; set; }
    public string DateFormat { get; set; }
}