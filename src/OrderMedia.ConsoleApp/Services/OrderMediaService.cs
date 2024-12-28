using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using OrderMedia.Interfaces;
using OrderMedia.Interfaces.Factories;

namespace OrderMedia.ConsoleApp.Services;
/// <summary>
/// Order Media service class.
/// </summary>
public class OrderMediaService : IHostedService
{
    private readonly ILogger<OrderMediaService> _logger;
    private readonly IIOService _ioService;
    private readonly IConfigurationService _configurationService;
    private readonly IMediaFactory _mediaFactory;
    private readonly IClassificationService _classificationService;
    private readonly IHostApplicationLifetime _hostApplicationLifetime;

    public OrderMediaService(
        ILogger<OrderMediaService> logger,
        IIOService ioService,
        IConfigurationService configurationService,
        IMediaFactory mediaFactoryService,
        IClassificationService classificationService,
        IHostApplicationLifetime hostApplicationLifetime)
    {
        _logger = logger;
        _ioService = ioService;
        _configurationService = configurationService;
        _mediaFactory = mediaFactoryService;
        _classificationService = classificationService;
        _hostApplicationLifetime = hostApplicationLifetime;
    }
    
    private void CreateMediaFolders()
    {
        _ioService.CreateFolder(_ioService.Combine(new string[] { _configurationService.GetMediaSourcePath(), _configurationService.GetImageFolderName() }));
        _ioService.CreateFolder(_ioService.Combine(new string[] { _configurationService.GetMediaSourcePath(), _configurationService.GetVideoFolderName() }));
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.StartClassification();

        CreateMediaFolders();

        Manage();

        
        // Thread.Sleep(100000);
        _hostApplicationLifetime.StopApplication();
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.EndClassification();
    }
    
    private void ManageMedia(params string[] extensions)
    {
        var allMedia = _ioService.GetFilesByExtensions(_configurationService.GetMediaSourcePath(), extensions);

        foreach (var media in allMedia)
        {
            var mediaObject = _mediaFactory.CreateMedia(media.FullName);

            _classificationService.Process(mediaObject);
        }
    }

    private void Manage()
    {
        // Images first because of livePhotos classification.
        ManageMedia(_configurationService.GetImageExtensions());
        ManageMedia(_configurationService.GetVideoExtensions());
    }
}

internal static partial class Log
{
    [LoggerMessage(Level = LogLevel.Information, Message = "Classification started")]
    public static partial void StartClassification(this ILogger<OrderMediaService> logger);
    
    [LoggerMessage(Level = LogLevel.Information, Message = "Classification ended")]
    public static partial void EndClassification(this ILogger<OrderMediaService> logger);
}