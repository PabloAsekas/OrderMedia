using System;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OrderMedia.Factories;
using OrderMedia.Handlers.Processor;
using OrderMedia.Interfaces;
using OrderMedia.Interfaces.Factories;
using OrderMedia.Configuration;
using OrderMedia.Handlers.CreatedDate;
using OrderMedia.Services;
using OrderMedia.Strategies.RenameStrategy;
using OrderMedia.Wrappers;

namespace OrderMedia.Extensions;

public static class ServiceCollectionExtension
{
    /// <summary>
    /// Adds OrderMedia services.
    /// </summary>
    /// <param name="services">Service Collection.</param>
    /// <returns><see cref="IServiceCollection"/> with all the services needed by the project.</returns>
    public static IServiceCollection AddOrderMedia(this IServiceCollection services)
    {
        services
            .AddScoped<IClassificationService, ClassificationService>()
            .AddScoped<ICopyComplementFilesService, CopyComplementFilesService>()
            .AddScoped<ICreatedDateExtractorService, CreatedDateExtractorService>()
            .AddScoped<IImageMetadataReader, ImageMetadataReaderWrapper>()
            .AddScoped<IIoWrapper, IoWrapper>()
            .AddScoped<IMediaFactory, MediaFactory>()
            .AddScoped<IMediaTypeService, MediaTypeService>()
            .AddScoped<IMetadataAggregatorService, MetadataAggregatorService>()
            .AddScoped<IMetadataExtractorService, MetadataExtractorService>()
            .AddScoped<IRandomizerService, RandomizerService>()
            .AddScoped<IAaeHelperService, AaeHelperService>()
            .AddScoped<DefaultRenameStrategy>()
            .AddScoped<IRenameStrategy, DefaultRenameStrategy>()
            .AddScoped<Insta360RenameStrategy>()
            .AddScoped<IRenameStrategy, Insta360RenameStrategy>()
            .AddScoped<IRenameStrategyFactory, RenameStrategyFactory>()
            .AddScoped<IXmpExtractorService, XmpExtractorService>();

        services.AddCreatedDateHandlers();

        services.AddProcessorHandlers();
            
        return services;
    }

    /// <summary>
    /// Configures OrderMedia.
    /// </summary>
    /// <param name="services">Service Collection.</param>
    /// <param name="configuration">Configuration.</param>
    /// <returns><see cref="IServiceCollection"/> with all the configuration needed by the project.</returns>
    public static IServiceCollection ConfigureOrderMedia(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddOptions<ClassificationFoldersOptions>()
            .BindConfiguration(ClassificationFoldersOptions.ConfigurationSection);
        services.AddOptions<ClassificationSettingsOptions>()
            .BindConfiguration(ClassificationSettingsOptions.ConfigurationSection);
        services.AddOptions<MediaExtensionsOptions>()
            .BindConfiguration(MediaExtensionsOptions.ConfigurationSection);
        services.AddOptions<MediaPathsOptions>()
            .BindConfiguration(MediaPathsOptions.ConfigurationSection);
        services.AddOptions<ClassificationProcessorsOptions>()
            .BindConfiguration(ClassificationProcessorsOptions.ConfigurationSection);

        return services;
    }

    private static IServiceCollection AddCreatedDateHandlers(this IServiceCollection services)
    {
        services
            .AddScoped<ExifIfd0DirectoryCreatedDateHandler>()
            .AddScoped<ExifSubIfdDirectoryCreatedDateHandler>()
            .AddScoped<FileMetadataDirectoryCreatedDateHandler>()
            .AddScoped<M01XmlCreatedDateHandler>()
            .AddScoped<QuickTimeMetadataHeaderDirectoryCreatedDateHandler>()
            .AddScoped<QuickTimeMovieHeaderDirectoryCreatedDateHandler>()
            .AddScoped<RegexCreatedDateHandler>()
            .AddScoped<XmpCreatedDateHandler>();

        services
            .AddTransient<Func<string, string, RegexCreatedDateHandler>>(
            sp =>
                (pattern, format) =>
                    new RegexCreatedDateHandler(sp.GetRequiredService<IIoWrapper>(), pattern, format));
        
        services.AddScoped<ICreatedDateChainFactory, CreatedDateChainFactory>();
        
        return services;
    }

    private static IServiceCollection AddProcessorHandlers(this IServiceCollection services)
    {
        services
            .AddScoped<CreatedDateAggregatorProcessorHandler>()
            .AddScoped<MoveAaeProcessorHandler>()
            .AddScoped<MoveLivePhotoProcessorHandler>()
            .AddScoped<MoveM01XmlProcessorHandler>()
            .AddScoped<MoveMediaProcessorHandler>()
            .AddScoped<MoveXmpProcessorHandler>();
        
        services.AddSingleton<IReadOnlyDictionary<string, IProcessorHandlerFactory>>(_ => new Dictionary<string, IProcessorHandlerFactory>
        {
            ["CreatedDateAggregatorProcessorHandler"] = new ProcessorHandlerFactory(sp => sp.GetRequiredService<CreatedDateAggregatorProcessorHandler>()),
            ["MoveAaeProcessorHandler"] = new ProcessorHandlerFactory(sp => sp.GetRequiredService<MoveAaeProcessorHandler>()),
            ["MoveLivePhotoProcessorHandler"] = new ProcessorHandlerFactory(sp => sp.GetRequiredService<MoveLivePhotoProcessorHandler>()),
            ["MoveM01XmlProcessorHandler"] = new ProcessorHandlerFactory(sp => sp.GetRequiredService<MoveM01XmlProcessorHandler>()),
            ["MoveMediaProcessorHandler"] = new ProcessorHandlerFactory(sp => sp.GetRequiredService<MoveMediaProcessorHandler>()),
            ["MoveXmpProcessorHandler"] = new ProcessorHandlerFactory(sp => sp.GetRequiredService<MoveXmpProcessorHandler>())
        });

        services.AddScoped<IProcessorHandlerFactory, ProcessorHandlerFactory>();
        services.AddScoped<IProcessorChainFactory, ProcessorChainFactory>();
        
        return services;
    }
}