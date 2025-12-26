using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OrderMedia.ConsoleApp.Configuration;
using OrderMedia.ConsoleApp.Extensions;
using OrderMedia.ConsoleApp.Interfaces;
using OrderMedia.Interfaces;
using OrderMedia.Interfaces.Factories;
using OrderMedia.Models;

namespace OrderMedia.ConsoleApp.Orchestrators;

public class ClassificationOrchestrator : BackgroundService
{
    private readonly ILogger<ClassificationOrchestrator> _logger;
    private readonly IIoWrapper _ioWrapper;
    private readonly MediaExtensionsSettings _mediaExtensionsSettings;
    private readonly ClassificationSettings _classificationSettings;
    private readonly IMediaFactory _mediaFactory;
    private readonly IClassificationService _classificationService;
    private readonly IProcessorChainFactory _processorChainFactory;
    private readonly IClassificationFolderPreparer _classificationFolderPreparer;
    

    public ClassificationOrchestrator(
        ILogger<ClassificationOrchestrator> logger,
        IIoWrapper ioWrapper,
        IOptions<MediaExtensionsSettings> mediaExtensionsOptions,
        IOptions<ClassificationSettings> classificationSettingsOptions,
        IMediaFactory mediaFactory,
        IClassificationService classificationService,
        IProcessorChainFactory processorChainFactory,
        IClassificationFolderPreparer classificationFolderPreparer)
    {
        _logger = logger;
        _ioWrapper = ioWrapper;
        _mediaExtensionsSettings = mediaExtensionsOptions.Value;
        _classificationSettings = classificationSettingsOptions.Value;
        _mediaFactory = mediaFactory;
        _classificationService = classificationService;
        _processorChainFactory = processorChainFactory;
        _classificationFolderPreparer = classificationFolderPreparer;
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.StartClassification();

        _classificationFolderPreparer.Prepare();

        // Images first because of livePhotos classification.
        ProcessMedia(_mediaExtensionsSettings.ImageExtensions);
        ProcessMedia(_mediaExtensionsSettings.VideoExtensions);
        
        _logger.EndClassification();
        
        return Task.CompletedTask;
    }
    
    private void ProcessMedia(params string[] extensions)
    {
        var allMediaFileInfo = _ioWrapper.GetFilesByExtensions(_classificationSettings.MediaSourcePath, extensions);

        foreach (var fileInfo in allMediaFileInfo)
        {
            var originalMedia = _mediaFactory.CreateMedia(fileInfo.FullName);
            
            var targetMedia = _classificationService.Classify(originalMedia);
            
            var request = new ProcessMediaRequest
            {
                Original = originalMedia,
                Target = targetMedia,
                OverwriteFiles = _classificationSettings.OverwriteFiles
            };

            var processor = _processorChainFactory.Build(originalMedia.Type);

            processor!.Process(request);
        }
    }
}