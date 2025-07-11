using System;
using Microsoft.Extensions.DependencyInjection;
using OrderMedia.Handlers.CreatedDate;
using OrderMedia.Interfaces;
using OrderMedia.Interfaces.Factories;
using OrderMedia.Interfaces.Handlers;

namespace OrderMedia.Factories;

public class CreatedDateChainFactory : ICreatedDateChainFactory
{
    private readonly IServiceProvider _serviceProvider;
    private readonly Func<string, string, RegexCreatedDateHandler> _regexCreatedDateHandlerFactory;

    public CreatedDateChainFactory(
        IServiceProvider serviceProvider,
        Func<string, string, RegexCreatedDateHandler> regexCreatedDateHandlerFactory
        )
    {
        _serviceProvider = serviceProvider;
        _regexCreatedDateHandlerFactory = regexCreatedDateHandlerFactory;
    }

    public ICreatedDateHandler CreateChain()
    {
        var imageMetadataReader = _serviceProvider.GetRequiredService<IImageMetadataReader>();
        var ioWrapper = _serviceProvider.GetRequiredService<IIoWrapper>();
        var xmpExtractorService = _serviceProvider.GetRequiredService<IXmpExtractorService>();
        
        var xmpHandler = new XmpCreatedDateHandler(ioWrapper, xmpExtractorService);
        var m01XmlHandler = new M01XmlCreatedDateHandler(ioWrapper);
        var exifSubIfdDirectoryHandler = new ExifSubIfdDirectoryCreatedDateHandler(imageMetadataReader);
        var exifIfd0DirectoryHandler = new ExifIfd0DirectoryCreatedDateHandler(imageMetadataReader);
        var quickTimeMetadataHeaderDirectoryHandler = new QuickTimeMetadataHeaderDirectoryCreatedDateHandler(imageMetadataReader);
        var quickTimeMovieHeaderDirectoryHandler = new QuickTimeMovieHeaderDirectoryCreatedDateHandler(imageMetadataReader);
        var fileMetadataDirectoryCreatedDateHandler = new FileMetadataDirectoryCreatedDateHandler(imageMetadataReader);
        var whatsAppHandler = _regexCreatedDateHandlerFactory(
            "[0-9]{4}-(0[1-9]|1[0-2])-(0[1-9]|[1-2][0-9]|3[0-1])-(0[0-9]|[1-2][0-9])-([0-5][0-9])-([0-5][0-9])",
            "yyyy-MM-dd-HH-mm-ss"); // Names like PHOTO-2024-04-09-19-45-45.jpg
        var insta360Handler = _regexCreatedDateHandlerFactory("[0-9]{4}(0[1-9]|1[0-2])(0[1-9]|[1-2][0-9]|3[0-1])_(0[0-9]|[1-2][0-9])([0-5][0-9])([0-5][0-9])", "yyyyMMdd_HHmmss"); // Names like IMG_20240713_164531.jpg
        var nextCloudHandler = _regexCreatedDateHandlerFactory("[0-9]{2}-(0[1-9]|1[0-2])-(0[1-9]|[1-2][0-9]|3[0-1]) (0[0-9]|[1-2][0-9])-([0-5][0-9])-([0-5][0-9])", "yy-MM-dd HH-mm-ss"); // Names like 24-08-03 18-29-44 1005.png
        
        xmpHandler
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
        
        return xmpHandler;
    }
}