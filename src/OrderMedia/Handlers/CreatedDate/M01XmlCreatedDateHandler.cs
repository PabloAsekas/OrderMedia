using System.Xml.Linq;
using OrderMedia.Interfaces;
using OrderMedia.Models;

namespace OrderMedia.Handlers.CreatedDate;

public class M01XmlCreatedDateHandler: BaseCreatedDateHandler
{
    private readonly IIOService _ioService;

    public M01XmlCreatedDateHandler(IIOService ioService)
    {
        _ioService = ioService;
    }

    public override CreatedDateInfo? GetCreatedDateInfo(string mediaPath)
    {
        var xmlFilePath = GetM01FilePath(mediaPath);

        var createdDate = GetDate(xmlFilePath);
        
        var createdDateInfo = CreateCreatedDateInfo(createdDate, "yyyy-MM-ddTHH:mm:sszzz");
        
        return createdDateInfo ?? base.GetCreatedDateInfo(mediaPath);
    }

    private string GetDate(string xmlFilePath)
    {
        var result = string.Empty;
        
        if (_ioService.FileExists(xmlFilePath))
        {
            var doc = XDocument.Load(xmlFilePath);

            XNamespace ns = "urn:schemas-professionalDisc:nonRealTimeMeta:ver.2.00";
            result = doc.Root?
                .Element(ns + "CreationDate")?
                .Attribute("value")?
                .Value;
        }

        return result;
    }
    
    private string GetM01FilePath(string mediaPath)
    {
        var folder = _ioService.GetDirectoryName(mediaPath);
        var nameWithoutExtension = _ioService.GetFileNameWithoutExtension(mediaPath);

        return _ioService.Combine(new[] { folder, $"{nameWithoutExtension}M01.xml" });
    }
}