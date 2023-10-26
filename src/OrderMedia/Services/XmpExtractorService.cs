using System.IO;
using OrderMedia.Interfaces;
using XmpCore;

namespace OrderMedia.Services
{
	public class XmpExtractorService : IXmpExtractorService
	{
        public string GetCreatedDate(string xmpFilePath)
        {
            var xmpFile = GetXmpMeta(xmpFilePath);

            return xmpFile.GetPropertyString("http://ns.adobe.com/exif/1.0/", "exif:DateTimeOriginal");
        }

        private IXmpMeta GetXmpMeta(string xmpFilePath)
        {
            IXmpMeta xmp;

            using (var stream = File.OpenRead(xmpFilePath))
            {
                xmp = XmpMetaFactory.Parse(stream);
            }

            return xmp;
        }
    }
}

