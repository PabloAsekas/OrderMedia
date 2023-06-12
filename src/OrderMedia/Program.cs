using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using OrderMedia.Factories;
using OrderMedia.Interfaces;
using OrderMedia.Services;
using OrderMedia.Services.CreatedDateExtractors;
using OrderMedia.Services.Processors;

namespace OrderMedia
{
    /// <summary>
    /// Main program class.
    /// </summary>
    public class Program
    {
        /// <summary>
        /// Main method.
        /// </summary>
        /// <param name="args">Arguments.</param>
        /// <returns>Nothing.</returns>
        public static void Main(string[] args)
        {
            var sp = CreateServiceProvider();

            var mcs = sp.GetRequiredService<OrderMediaService>();

            mcs.Run();
        }

        private static ServiceProvider CreateServiceProvider()
        {
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .Build();

            var serviceCollection = new ServiceCollection()
                .AddLogging(configure => configure.AddConsole())
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
                .AddScoped<IProcessor, MainProcessor>()
                .AddScoped<AaeProcessor>()
                .AddScoped<IProcessor, AaeProcessor>(s => s.GetService<AaeProcessor>())
                .AddScoped<LivePhotoProcessor>()
                .AddScoped<IProcessor, LivePhotoProcessor>(s => s.GetService<LivePhotoProcessor>())
                .AddScoped<IProcessorFactory, ProcessorFactory>()
                .AddScoped<OrderMediaService>()
                .AddScoped<IRandomizerService, RandomizerService>()
                .AddScoped<IRenameService, RenameService>()
                .AddSingleton<IConfiguration>(configuration);

            return serviceCollection.BuildServiceProvider();
        }
    }
}