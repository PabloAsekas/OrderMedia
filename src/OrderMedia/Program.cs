using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
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
        public static async Task Main(string[] args)
        {
            using IHost host = CreateHostBuilder(args).Build();

            await host.RunAsync();
        }

        private static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((hostingContext, configuration) =>
                {
                    configuration.Sources.Clear();

                    configuration.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

                    configuration.AddCommandLine(args);
                })
                .ConfigureServices((hostContext, services) =>
                {
                    // Add IOService.
                    services.AddScoped<IIOService, IOService>();

                    // Add ConfigurationService
                    services.AddScoped<IConfigurationService, ConfigurationService>();

                    // Add MediaFactoryService
                    services.AddScoped<IMediaFactoryService, MediaFactoryService>();

                    // Add MediaClassificationService.
                    services.AddHostedService<MediaClassificationService>();
                })
                .ConfigureLogging((hostContext, configLogging) =>
                {
                    configLogging.AddConsole();
                })
                .UseConsoleLifetime();
        }
    }
}