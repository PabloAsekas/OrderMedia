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

        public string GetValue(string xmpFilePath, string schemaName, string propertyName)
        {
            if (!_ioService.FileExists(xmpFilePath))
            {
                return null;
            }
            
            var xmpFile = GetXmpMeta(xmpFilePath);
            
            return xmpFile.GetPropertyString(schemaName, propertyName);
        }

        private static IXmpMeta GetXmpMeta(string xmpFilePath)
        {
            using var stream = File.OpenRead(xmpFilePath);
            var xmp = XmpMetaFactory.Parse(stream);

            return xmp;
        }
    }
}

