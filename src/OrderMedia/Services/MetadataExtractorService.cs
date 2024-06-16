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
            var xmpHandler = new XmpCreatedDateHandler(ioService, xmpExtractorService);
            var exifSubIfdDirectoryHandler = new ExifSubIfdDirectoryCreatedDateHandler(imageMetadataReader);
            var exifIfd0DirectoryHandler = new ExifIfd0DirectoryCreatedDateHandler(imageMetadataReader);
            var quickTimeMetadataHeaderDirectoryHandler = new QuickTimeMetadataHeaderDirectoryCreatedDateHandler(imageMetadataReader);
            var whatsAppHandler = new WhatsAppCreatedDateHandler(ioService);

            xmpHandler
                .SetNext(exifSubIfdDirectoryHandler)
                .SetNext(exifIfd0DirectoryHandler)
                .SetNext(quickTimeMetadataHeaderDirectoryHandler)
                .SetNext(whatsAppHandler);

            _createdDateHandler = xmpHandler;
        }
        public CreatedDateInfo GetCreatedDate(string mediaPath)
        {
            return _createdDateHandler.GetCreatedDateInfo(mediaPath);
        }
    }
}

