using System.Globalization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using OrderMedia.ConsoleApp.Configuration;
using OrderMedia.ConsoleApp.Extensions;
using OrderMedia.DigiKam.Extensions;
using OrderMedia.Extensions;
using OrderMedia.Immich.Extensions;

namespace OrderMedia.ConsoleApp;

/// <summary>
/// Main program class.
/// </summary>
public class Program
{
    public static async Task Main(string[] args)
    {
        ConfigureDefaultCulture();
        
        var hostBuilder = CreateAppBuilder(args);
        IHost host = hostBuilder.Build();
        await host.RunAsync();
    }

    private static void ConfigureDefaultCulture()
    {
        var defaultCulture = new CultureInfo("en-US");

        Thread.CurrentThread.CurrentCulture ??= defaultCulture;
    }

    internal static HostApplicationBuilder CreateAppBuilder(string[] args)
    {
        HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);
        builder.Logging.AddConsole();
        builder.Services.ConfigureApplication();
        builder.Services.AddConsoleAppServices();
        builder.Services.AddOrderMedia();
        builder.Services.AddDigiKam(options =>
        {
            var syncOptions = new SynchronizationSettings();
            builder.Configuration.GetSection(SynchronizationSettings.ConfigurationSection).Bind(syncOptions);
            
            options.ConnectionString = syncOptions.DigiKam.ConnectionString;
        });
        builder.Services.AddImmich(options =>
        {
            var syncOptions = new SynchronizationSettings();
            builder.Configuration.GetSection(SynchronizationSettings.ConfigurationSection).Bind(syncOptions);
            
            options.Url = syncOptions.Immich.Url;
            options.ApiKey = syncOptions.Immich.ApiKey;
        });
        
        return builder;
    }
}
