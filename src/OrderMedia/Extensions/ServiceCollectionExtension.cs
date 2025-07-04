using Microsoft.Extensions.DependencyInjection;
using OrderMedia.Factories;
using OrderMedia.Handlers.Processor;
using OrderMedia.Interfaces;
using OrderMedia.Interfaces.Factories;
using OrderMedia.Interfaces.Handlers;
using OrderMedia.Services;
using OrderMedia.Strategies.RenameStrategy;
using OrderMedia.Wrappers;

namespace OrderMedia.Extensions;

public static class ServiceCollectionExtension
{
    /// <summary>
    /// Adds OrderMedia services and configurations.
    /// </summary>
    /// <param name="services">Service Collection.</param>
    /// <returns><see cref="IServiceCollection"/> with all the services needed by the project.</returns>
    public static IServiceCollection AddOrderMediaServiceClient(this IServiceCollection services)
    {
        services
            .AddScoped<IClassificationService, ClassificationService>()
            .AddScoped<IConfigurationService, ConfigurationService>()
            .AddScoped<ICopyComplementFilesService, CopyComplementFilesService>()
            .AddScoped<ICreatedDateExtractorService, CreatedDateExtractorService>()
            .AddScoped<IImageMetadataReader, ImageMetadataReaderWrapper>()
            .AddScoped<IIOService, IOService>()
            .AddScoped<IMediaFactory, MediaFactory>()
            .AddScoped<IMediaTypeService, MediaTypeService>()
            .AddScoped<IMetadataAggregatorService, MetadataAggregatorService>()
            .AddScoped<IMetadataExtractorService, MetadataExtractorService>()
            .AddScoped<MoveMediaProcessorHandler>()
            .AddScoped<IProcessorHandler, MoveMediaProcessorHandler>(s => s.GetRequiredService<MoveMediaProcessorHandler>())
            .AddScoped<MoveAaeProcessorHandler>()
            .AddScoped<IProcessorHandler, MoveAaeProcessorHandler>(s => s.GetRequiredService<MoveAaeProcessorHandler>())
            .AddScoped<MoveLivePhotoProcessorHandler>()
            .AddScoped<IProcessorHandler, MoveLivePhotoProcessorHandler>(s => s.GetRequiredService<MoveLivePhotoProcessorHandler>())
            .AddScoped<MoveXmpProcessorHandler>()
            .AddScoped<IProcessorHandler, MoveXmpProcessorHandler>(s => s.GetRequiredService<MoveXmpProcessorHandler>())
            .AddScoped<CreatedDateAggregatorProcessorHandler>()
            .AddScoped<IProcessorHandler, CreatedDateAggregatorProcessorHandler>(s => s.GetRequiredService<CreatedDateAggregatorProcessorHandler>()).AddScoped<MoveM01XmlProcessorHandler>()
            .AddScoped<IProcessorHandler, MoveM01XmlProcessorHandler>(s => s.GetRequiredService<MoveM01XmlProcessorHandler>())
            .AddScoped<IProcessorHandlerFactory, ProcessorHandlerFactory>()
            .AddScoped<IRandomizerService, RandomizerService>()
            .AddScoped<IAaeHelperService, AaeHelperService>()
            .AddScoped<DefaultRenameStrategy>()
            .AddScoped<IRenameStrategy, DefaultRenameStrategy>()
            .AddScoped<Insta360RenameStrategy>()
            .AddScoped<IRenameStrategy, Insta360RenameStrategy>()
            .AddScoped<IRenameStrategyFactory, RenameStrategyFactory>()
            .AddScoped<IXmpExtractorService, XmpExtractorService>();
            
        return services;
    }
}