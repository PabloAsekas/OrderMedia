using System.IO;
using OrderMedia.Interfaces;
using XmpCore;

namespace OrderMedia.Services
{
	public class XmpExtractorService : IXmpExtractorService
    {
        private readonly IIOService _ioService;

        public XmpExtractorService(IIOService ioService)
        {
            _ioService = ioService;
        }

        public string GetCreatedDate(string xmpFilePath)
        {
            if (!_ioService.FileExists(xmpFilePath))
            {
                return null;
            }
            
            var xmpFile = GetXmpMeta(xmpFilePath);
            
            return xmpFile.GetPropertyString("http://ns.adobe.com/exif/1.0/", "exif:DateTimeOriginal");
        }

        private static IXmpMeta GetXmpMeta(string xmpFilePath)
        {
            using var stream = File.OpenRead(xmpFilePath);
            var xmp = XmpMetaFactory.Parse(stream);

            return xmp;
        }
    }
}

