// <copyright file="Program.cs" company="Pablo Bermejo">
// Copyright (c) Pablo Bermejo. All rights reserved.
// </copyright>

namespace OrderMedia
{
    using System.Threading.Tasks;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;
    using OrderMedia.Interfaces;
    using OrderMedia.Services;

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
                    var serviceProvider = services.BuildServiceProvider();

                    var configuration = serviceProvider.GetService<IConfiguration>();

                    // Add IOService.
                    services.AddScoped<IIOService, IOService>(serviceProvider =>
                    {
                        return new IOService(configuration.GetSection("originalPath").Value, configuration.GetSection("imgFolder").Value, configuration.GetSection("vidFolder").Value);
                    });

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