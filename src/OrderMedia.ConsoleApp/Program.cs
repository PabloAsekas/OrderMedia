using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using OrderMedia.ConsoleApp.Services;
using OrderMedia.Extensions;

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
                .AddOrderMediaServiceClient()
                .AddScoped<OrderMediaService>()
                .AddSingleton<IConfiguration>(configuration);

            return serviceCollection.BuildServiceProvider();
        }
    }
}