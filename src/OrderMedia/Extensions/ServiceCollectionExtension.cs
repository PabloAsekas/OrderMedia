
using Microsoft.Extensions.DependencyInjection;
using OrderMedia.Factories;
using OrderMedia.Interfaces;
using OrderMedia.Services;
using OrderMedia.Services.Processors;
using OrderMedia.Wrappers;

namespace OrderMedia.Extensions
{
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
                .AddScoped<MainProcessor>()
                .AddScoped<IProcessor, MainProcessor>(s => s.GetService<MainProcessor>())
                .AddScoped<AaeProcessor>()
                .AddScoped<IProcessor, AaeProcessor>(s => s.GetService<AaeProcessor>())
                .AddScoped<LivePhotoProcessor>()
                .AddScoped<IProcessor, LivePhotoProcessor>(s => s.GetService<LivePhotoProcessor>())
                .AddScoped<XmpProcessor>()
                .AddScoped<IProcessor, XmpProcessor>(s => s.GetService<XmpProcessor>())
                .AddScoped<CreatedDateProcessor>()
                .AddScoped<IProcessor, CreatedDateProcessor>(s => s.GetService<CreatedDateProcessor>())
                .AddScoped<IProcessorFactory, ProcessorFactory>()
                .AddScoped<IRandomizerService, RandomizerService>()
                .AddScoped<IRenameService, RenameService>()
                .AddScoped<IXmpExtractorService, XmpExtractorService>();
            
            return services;
        }
    }
}

