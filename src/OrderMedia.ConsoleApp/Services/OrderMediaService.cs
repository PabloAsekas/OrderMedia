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
public class OrderMediaService : IHostedService
{
    private readonly ILogger<OrderMediaService> _logger;
    private readonly IIoWrapper _ioWrapper;
    private readonly IMediaFactory _mediaFactory;
    private readonly IClassificationService _classificationService;
    private readonly IHostApplicationLifetime _hostApplicationLifetime;
    private readonly MediaPathsOptions _mediaPathsOptions;
    private readonly MediaExtensionsOptions _mediaExtensionsOptions;
    private readonly ClassificationFoldersOptions _classificationFoldersOptions;

    public OrderMediaService(
        ILogger<OrderMediaService> logger,
        IIoWrapper ioWrapper,
        IMediaFactory mediaFactoryService,
        IClassificationService classificationService,
        IHostApplicationLifetime hostApplicationLifetime,
        IOptions<MediaPathsOptions> mediaPathsOptions,
        IOptions<MediaExtensionsOptions> mediaExtensionsOptions,
        IOptions<ClassificationFoldersOptions> classificationFoldersOptions)
    {
        _logger = logger;
        _ioWrapper = ioWrapper;
        _mediaFactory = mediaFactoryService;
        _classificationService = classificationService;
        _hostApplicationLifetime = hostApplicationLifetime;
        _mediaPathsOptions = mediaPathsOptions.Value;
        _mediaExtensionsOptions = mediaExtensionsOptions.Value;
        _classificationFoldersOptions = classificationFoldersOptions.Value;
    }
    
    private void CreateMediaFolders()
    {
        _ioWrapper.CreateFolder(_ioWrapper.Combine([
            _mediaPathsOptions.MediaSourcePath,
            _classificationFoldersOptions.ImageFolderName
        ]));
        _ioWrapper.CreateFolder(_ioWrapper.Combine([
            _mediaPathsOptions.MediaSourcePath,
            _classificationFoldersOptions.VideoFolderName
        ]));
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.StartClassification();

        CreateMediaFolders();

        Manage();
        
        _hostApplicationLifetime.StopApplication();
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.EndClassification();
    }
    
    private void ManageMedia(params string[] extensions)
    {
        var allMedia = _ioWrapper.GetFilesByExtensions(_mediaPathsOptions.MediaSourcePath, extensions);

        foreach (var media in allMedia)
        {
            var mediaObject = _mediaFactory.CreateMedia(media.FullName);

            _classificationService.Process(mediaObject);
        }
    }

    private void Manage()
    {
        // Images first because of livePhotos classification.
        ManageMedia(_mediaExtensionsOptions.ImageExtensions);
        ManageMedia(_mediaExtensionsOptions.VideoExtensions);
    }
}

internal static partial class Log
{
    [LoggerMessage(Level = LogLevel.Information, Message = "Classification started")]
    public static partial void StartClassification(this ILogger<OrderMediaService> logger);
    
    [LoggerMessage(Level = LogLevel.Information, Message = "Classification ended")]
    public static partial void EndClassification(this ILogger<OrderMediaService> logger);
}