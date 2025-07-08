using System.Xml.Linq;
using OrderMedia.Interfaces;
using OrderMedia.Models;

namespace OrderMedia.Handlers.CreatedDate;

public class M01XmlCreatedDateHandler: BaseCreatedDateHandler
{
    private readonly IIoWrapper _ioWrapper;

    public M01XmlCreatedDateHandler(IIoWrapper ioWrapper)
    {
        _ioWrapper = ioWrapper;
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
        
        if (_ioWrapper.FileExists(xmlFilePath))
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
        var folder = _ioWrapper.GetDirectoryName(mediaPath);
        var nameWithoutExtension = _ioWrapper.GetFileNameWithoutExtension(mediaPath);

        return _ioWrapper.Combine(new[] { folder, $"{nameWithoutExtension}M01.xml" });
    }
}