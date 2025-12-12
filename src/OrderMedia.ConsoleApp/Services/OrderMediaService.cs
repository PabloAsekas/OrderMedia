using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OrderMedia.Interfaces;
using OrderMedia.Interfaces.Factories;
using OrderMedia.Configuration;

namespace OrderMedia.ConsoleApp.Services;
/// <summary>
/// Order Media service class.
/// </summary>
public class OrderMediaService : BackgroundService
{
    private readonly ILogger<OrderMediaService> _logger;
    private readonly IIoWrapper _ioWrapper;
    private readonly IMediaFactory _mediaFactory;
    private readonly IClassificationService _classificationService;
    private readonly IHostApplicationLifetime _hostApplicationLifetime;
    private readonly MediaExtensionsSettings _mediaExtensionsSettings;
    private readonly ClassificationSettings _classificationSettings;

    public OrderMediaService(
        ILogger<OrderMediaService> logger,
        IIoWrapper ioWrapper,
        IMediaFactory mediaFactoryService,
        IClassificationService classificationService,
        IHostApplicationLifetime hostApplicationLifetime,
        IOptions<MediaExtensionsSettings> mediaExtensionsOptions,
        IOptions<ClassificationSettings> classificationSettingsOptions)
    {
        _logger = logger;
        _ioWrapper = ioWrapper;
        _mediaFactory = mediaFactoryService;
        _classificationService = classificationService;
        _hostApplicationLifetime = hostApplicationLifetime;
        _mediaExtensionsSettings = mediaExtensionsOptions.Value;
        _classificationSettings = classificationSettingsOptions.Value;
    }

    protected async override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.StartClassification();

        CreateMediaFolders();

        Manage();
        
        _logger.EndClassification();
    }
    
    private void CreateMediaFolders()
    {
        _ioWrapper.CreateFolder(_ioWrapper.Combine([
            _classificationSettings.MediaSourcePath,
            _classificationSettings.Folders.ImageFolderName
        ]));
        _ioWrapper.CreateFolder(_ioWrapper.Combine([
            _classificationSettings.MediaSourcePath,
            _classificationSettings.Folders.VideoFolderName
        ]));
    }
    
    private void Manage()
    {
        // Images first because of livePhotos classification.
        ManageMedia(_mediaExtensionsSettings.ImageExtensions);
        ManageMedia(_mediaExtensionsSettings.VideoExtensions);
    }
    
    private void ManageMedia(params string[] extensions)
    {
        var allMedia = _ioWrapper.GetFilesByExtensions(_classificationSettings.MediaSourcePath, extensions);

        foreach (var media in allMedia)
        {
            var mediaObject = _mediaFactory.CreateMedia(media.FullName);

            _classificationService.Process(mediaObject);
        }
    }
}

internal static partial class Log
{
    [LoggerMessage(Level = LogLevel.Information, Message = "Classification started")]
    public static partial void StartClassification(this ILogger<OrderMediaService> logger);
    
    [LoggerMessage(Level = LogLevel.Information, Message = "Classification ended")]
    public static partial void EndClassification(this ILogger<OrderMediaService> logger);
}