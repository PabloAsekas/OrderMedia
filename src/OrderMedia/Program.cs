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

                    // Add MetadataService
                    //services.AddScoped<IMetadataService, MetadataService>();

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

        /*static void Main(string[] args)
        {
            string originalPath = @"/Users/pabloasekas/Downloads/fotis/"; // Siempre tiene que llevar el / final.

            DirectoryInfo d = new DirectoryInfo(originalPath);

            string imgFolder = Path.Combine(originalPath, "img");
            System.IO.Directory.CreateDirectory(imgFolder);

            // FileInfo[] ImgFiles = d.GetFiles("*.heic", SearchOption.TopDirectoryOnly); //Getting Img files

            List<FileInfo> ImgFiles = d.GetFilesByExtensions(".heic", ".jpg", ".jpeg", ".png", ".gif").ToList();

            foreach (FileInfo file in ImgFiles) {
                IEnumerable<MetadataExtractor.Directory> directories = ImageMetadataReader.ReadMetadata(file.FullName);

                var subIfdDirectory = directories.OfType<ExifSubIfdDirectory>().FirstOrDefault();
                var dateTime = subIfdDirectory?.GetDescription(ExifDirectoryBase.TagDateTimeOriginal);

                DateTime.TryParseExact(dateTime, "yyyy:MM:dd HH:mm:ss", new CultureInfo("es-ES", false), System.Globalization.DateTimeStyles.None, out DateTime date);

                string dateTimeOriginalFolder = Path.Combine(imgFolder, date.ToString("yyyy-MM-dd"));
                System.IO.Directory.CreateDirectory(dateTimeOriginalFolder);

                string newImageLocation = Path.Combine(dateTimeOriginalFolder, file.Name);
                File.Move(file.FullName, newImageLocation);

                string imageNameWithoutExtension = Path.GetFileNameWithoutExtension(file.FullName);
                string videoName = $"{imageNameWithoutExtension}.mov";
                string videoLocation = Path.Combine(originalPath, videoName);

                if (File.Exists(videoLocation))
                {
                    string newVideoLocation = Path.Combine(dateTimeOriginalFolder, videoName);
                    File.Move(videoLocation, newVideoLocation);
                }
            }

            Console.WriteLine("Hecho");

            Console.ReadLine();
        }*/
    }
}