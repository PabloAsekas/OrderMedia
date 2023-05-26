using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using OrderMedia.Interfaces;
using OrderMedia.Services;

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

            var mcs = sp.GetRequiredService<MediaClassificationService>();

            mcs.Run();
        }

        private static ServiceProvider CreateServiceProvider()
        {
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .Build();

            var serviceCollection = new ServiceCollection()
                .AddLogging(configure => configure.AddConsole())
                .AddScoped<IIOService, IOService>() // Add IOService.
                .AddScoped<IConfigurationService, ConfigurationService>() // Add ConfigurationService
                .AddScoped<IMediaFactoryService, MediaFactoryService>() // Add MediaFactoryService
                .AddScoped<MediaClassificationService>()
                .AddSingleton<IConfiguration>(configuration);

            return serviceCollection.BuildServiceProvider();
        }
    }
}