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
            var fileMetadataDirectoryCreatedDateHandler = new FileMetadataDirectoryCreatedDateHandler(imageMetadataReader);
            var quickTimeMetadataHeaderDirectoryHandler = new QuickTimeMetadataHeaderDirectoryCreatedDateHandler(imageMetadataReader);
            var quickTimeMovieHeaderDirectoryHandler = new QuickTimeMovieHeaderDirectoryCreatedDateHandler(imageMetadataReader);
            var whatsAppHandler = new RegexCreatedDateHandler(ioService, "[0-9]{4}-(0[1-9]|1[0-2])-(0[1-9]|[1-2][0-9]|3[0-1])-(0[0-9]|[1-2][0-9])-([0-5][0-9])-([0-5][0-9])", "yyyy-MM-dd-HH-mm-ss"); // Names like PHOTO-2024-04-09-19-45-45.jpg
            var insta360Handler = new RegexCreatedDateHandler(ioService,
                "[0-9]{4}(0[1-9]|1[0-2])(0[1-9]|[1-2][0-9]|3[0-1])_(0[0-9]|[1-2][0-9])([0-5][0-9])([0-5][0-9])", "yyyyMMdd_HHmmss"); // Names like IMG_20240713_164531.jpg

            xmpHandler
                .SetNext(exifSubIfdDirectoryHandler)
                .SetNext(exifIfd0DirectoryHandler)
                .SetNext(fileMetadataDirectoryCreatedDateHandler)
                .SetNext(quickTimeMetadataHeaderDirectoryHandler)
                .SetNext(quickTimeMovieHeaderDirectoryHandler)
                .SetNext(whatsAppHandler)
                .SetNext(insta360Handler);

            _createdDateHandler = xmpHandler;
        }
        
        public CreatedDateInfo GetCreatedDate(string mediaPath)
        {
            return _createdDateHandler.GetCreatedDateInfo(mediaPath);
        }
    }
}

