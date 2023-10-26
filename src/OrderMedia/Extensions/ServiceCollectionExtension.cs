using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OrderMedia.Factories;
using OrderMedia.Interfaces;
using OrderMedia.Services;
using OrderMedia.Services.CreatedDateExtractors;
using OrderMedia.Services.Processors;

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
                .AddScoped<ImageCreatedDateExtractor>()
                .AddScoped<ICreatedDateExtractor, ImageCreatedDateExtractor>(s => s.GetService<ImageCreatedDateExtractor>())
                .AddScoped<RawCreatedDateExtractor>()
                .AddScoped<ICreatedDateExtractor, RawCreatedDateExtractor>(s => s.GetService<RawCreatedDateExtractor>())
                .AddScoped<VideoCreatedDateExtractor>()
                .AddScoped<ICreatedDateExtractor, VideoCreatedDateExtractor>(s => s.GetService<VideoCreatedDateExtractor>())
                .AddScoped<WhatsAppCreatedDateExtractor>()
                .AddScoped<ICreatedDateExtractor, WhatsAppCreatedDateExtractor>(s => s.GetService<WhatsAppCreatedDateExtractor>())
                .AddScoped<ICreatedDateExtractorsFactory, CreatedDateExtractorsFactory>()
                .AddScoped<IIOService, IOService>()
                .AddScoped<IMediaFactory, MediaFactory>()
                .AddScoped<IMediaTypeService, MediaTypeService>()
                .AddScoped<IMetadataExtractorService, MetadataExtractorService>()
                .AddScoped<MainProcessor>()
                .AddScoped<IProcessor, MainProcessor>(s => s.GetService<MainProcessor>())
                .AddScoped<AaeProcessor>()
                .AddScoped<IProcessor, AaeProcessor>(s => s.GetService<AaeProcessor>())
                .AddScoped<LivePhotoProcessor>()
                .AddScoped<IProcessor, LivePhotoProcessor>(s => s.GetService<LivePhotoProcessor>())
                .AddScoped<XmpProcessor>()
                .AddScoped<IProcessor, XmpProcessor>(s => s.GetService<XmpProcessor>())
                .AddScoped<IProcessorFactory, ProcessorFactory>()
                .AddScoped<IRandomizerService, RandomizerService>()
                .AddScoped<IRenameService, RenameService>()
                .AddScoped<IXmpExtractorService, XmpExtractorService>();

            return services;
        }
    }
}

