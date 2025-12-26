using System.Globalization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using OrderMedia.ConsoleApp.Extensions;
using OrderMedia.ConsoleApp.Services;
using OrderMedia.Extensions;

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
        HostApplicationBuilder builder = Host.CreateApplicationBuilder();
        builder.Logging.AddConsole();
        builder.Services.ConfigureApplication();
        builder.Services.AddConsoleAppServices();
        builder.Services.AddOrderMedia();
        
        return builder;
    }
}
