using OrderMedia.Handlers.CreatedDate;
using OrderMedia.Interfaces;
using OrderMedia.Interfaces.Handlers;
using OrderMedia.Models;

namespace OrderMedia.Services
{
    public class MetadataExtractorService : IMetadataExtractorService
    {
        private readonly ICreatedDateHandler _createdDateHandler;
        
        public MetadataExtractorService(IImageMetadataReader imageMetadataReader, IIOService ioService, IXmpExtractorService xmpExtractorService)
        {
            var xmpExifHandler = new XmpCreatedDateHandler(
                ioService,
                xmpExtractorService,
                new XmpInfo
                {
                    SchemaName = "http://ns.adobe.com/exif/1.0/",
                    PropertyName = "exif:DateTimeOriginal",
                    DateFormat = "yyyy-MM-ddTHH:mm:ss",
                });
            var xmpXmpHandler = new XmpCreatedDateHandler(
                ioService,
                xmpExtractorService,
                new XmpInfo
                {
                    SchemaName = "http://ns.adobe.com/exif/1.0/",
                    PropertyName = "exif:DateTimeOriginal",
                    DateFormat = "yyyy-MM-ddTHH:mm:sszzz",
                });
            var m01XmlHandler = new M01XmlCreatedDateHandler(ioService);
            var exifSubIfdDirectoryHandler = new ExifSubIfdDirectoryCreatedDateHandler(imageMetadataReader);
            var exifIfd0DirectoryHandler = new ExifIfd0DirectoryCreatedDateHandler(imageMetadataReader);
            var quickTimeMetadataHeaderDirectoryHandler = new QuickTimeMetadataHeaderDirectoryCreatedDateHandler(imageMetadataReader);
            var quickTimeMovieHeaderDirectoryHandler = new QuickTimeMovieHeaderDirectoryCreatedDateHandler(imageMetadataReader);
            var fileMetadataDirectoryCreatedDateHandler = new FileMetadataDirectoryCreatedDateHandler(imageMetadataReader);
            var whatsAppHandler = new RegexCreatedDateHandler(ioService, "[0-9]{4}-(0[1-9]|1[0-2])-(0[1-9]|[1-2][0-9]|3[0-1])-(0[0-9]|[1-2][0-9])-([0-5][0-9])-([0-5][0-9])", "yyyy-MM-dd-HH-mm-ss"); // Names like PHOTO-2024-04-09-19-45-45.jpg
            var insta360Handler = new RegexCreatedDateHandler(ioService, "[0-9]{4}(0[1-9]|1[0-2])(0[1-9]|[1-2][0-9]|3[0-1])_(0[0-9]|[1-2][0-9])([0-5][0-9])([0-5][0-9])", "yyyyMMdd_HHmmss"); // Names like IMG_20240713_164531.jpg
            var nextCloudHandler = new RegexCreatedDateHandler(ioService, "[0-9]{2}-(0[1-9]|1[0-2])-(0[1-9]|[1-2][0-9]|3[0-1]) (0[0-9]|[1-2][0-9])-([0-5][0-9])-([0-5][0-9])", "yy-MM-dd HH-mm-ss"); // Names like 24-08-03 18-29-44 1005.png
            
            xmpExifHandler
                .SetNext(xmpXmpHandler)
                .SetNext(m01XmlHandler)
                .SetNext(insta360Handler)
                .SetNext(whatsAppHandler)
                .SetNext(nextCloudHandler)
                .SetNext(exifSubIfdDirectoryHandler)
                .SetNext(exifIfd0DirectoryHandler)
                .SetNext(quickTimeMetadataHeaderDirectoryHandler)
                .SetNext(quickTimeMovieHeaderDirectoryHandler)
                // .SetNext(fileMetadataDirectoryCreatedDateHandler)
                ;
            
            _createdDateHandler = xmpExifHandler;
        }
        
        public CreatedDateInfo? GetCreatedDate(string mediaPath)
        {
            return _createdDateHandler.GetCreatedDateInfo(mediaPath);
        }
    }
}

