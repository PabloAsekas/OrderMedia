using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OrderMedia.ConsoleApp.Configuration;
using OrderMedia.ConsoleApp.Extensions;
using OrderMedia.ConsoleApp.Interfaces;
using OrderMedia.Interfaces;
using OrderMedia.Interfaces.Factories;
using OrderMedia.Models;

namespace OrderMedia.ConsoleApp.Orchestrators;

public class RenamingOrchestrator : IOrchestrator
{
    public const string ServiceName = "RenamingOrchestrator";
    
    private readonly ILogger<RenamingOrchestrator> _logger;
    private readonly MediaExtensionsSettings _mediaExtensionsSettings;
    private readonly RenamingSettings _renamingSettings;
    private readonly IIoWrapper _ioWrapper;
    private readonly IMediaFactory _mediaFactory;
    private readonly IRenamingService _renamingService;
    private readonly IProcessorChainFactory _processorChainFactory;
    private readonly IRenamingValidatorService _renamingValidatorService;
    
    public RenamingOrchestrator(
        ILogger<RenamingOrchestrator> logger,
        IOptions<MediaExtensionsSettings> mediaExtensionsSettings,
        IOptions<RenamingSettings> renamingSettings,
        IIoWrapper ioWrapper,
        IMediaFactory mediaFactory,
        IRenamingService renamingService,
        IProcessorChainFactory processorChainFactory, 
        IRenamingValidatorService renamingValidatorService)
    {
        _logger = logger;
        _ioWrapper = ioWrapper;
        _mediaFactory = mediaFactory;
        _renamingService = renamingService;
        _processorChainFactory = processorChainFactory;
        _renamingValidatorService = renamingValidatorService;
        _mediaExtensionsSettings = mediaExtensionsSettings.Value;
        _renamingSettings = renamingSettings.Value;
    }

    public Task RunAsync(CancellationToken stoppingToken)
    {
        _logger.StartRenaming();

        // Images first so it process live photos videos.
        ProcessMedia(_mediaExtensionsSettings.ImageExtensions);
        ProcessMedia(_mediaExtensionsSettings.VideoExtensions);
        
        _logger.EndRenaming();
        
        return Task.CompletedTask;
    }

    private void ProcessMedia(params string[] extensions)
    {
        var allMediaFileInfo = _ioWrapper.GetAllFilesByExtensions(_renamingSettings.MediaSourcePath, extensions);

        foreach (var fileInfo in allMediaFileInfo)
        {
            var originalMedia = _mediaFactory.CreateMedia(fileInfo.FullName);
            
            var isValid = _renamingValidatorService.ValidateMedia(originalMedia);
            
            if (!isValid)
            {
                _ioWrapper.RejectMedia(fileInfo.FullName);
                continue;
            }
            
            var targetMedia = _renamingService.Rename(originalMedia);
            
            var request = new ProcessMediaRequest
            {
                Original = originalMedia,
                Target = targetMedia
            };

            var processor = _processorChainFactory.Build(originalMedia.Type);
            
            processor!.Process(request);
        }
    }
}