using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OrderMedia.Interfaces;
using OrderMedia.Interfaces.Factories;
using OrderMedia.Configuration;
using OrderMedia.Enums;
using OrderMedia.Models;

namespace OrderMedia.ConsoleApp.Services;
/// <summary>
/// Order Media service class.
/// </summary>
public class ClassificationService : BackgroundService
{
    private readonly ILogger<ClassificationService> _logger;
    private readonly IIoWrapper _ioWrapper;
    private readonly IMediaFactory _mediaFactory;
    private readonly IProcessorChainFactory _processorChainFactory;
    private readonly MediaExtensionsSettings _mediaExtensionsSettings;
    private readonly ClassificationSettings _classificationSettings;
    private readonly IRenameStrategyFactory _renameStrategyFactory;

    public ClassificationService(
        ILogger<ClassificationService> logger,
        IIoWrapper ioWrapper,
        IMediaFactory mediaFactoryService,
        IProcessorChainFactory processorChainFactory,
        IOptions<MediaExtensionsSettings> mediaExtensionsOptions,
        IOptions<ClassificationSettings> classificationSettingsOptions,
        IRenameStrategyFactory renameStrategyFactory)
    {
        _logger = logger;
        _ioWrapper = ioWrapper;
        _mediaFactory = mediaFactoryService;
        _processorChainFactory = processorChainFactory;
        _renameStrategyFactory = renameStrategyFactory;
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
        var allMediaFileInfo = _ioWrapper.GetFilesByExtensions(_classificationSettings.MediaSourcePath, extensions);

        foreach (var mediaFileInfo in allMediaFileInfo)
        {
            var fromMedia = _mediaFactory.CreateMedia(mediaFileInfo.FullName);
            
            var toMedia = CreateClassificationMedia(fromMedia);

            var request = new ProcessMediaRequest()
            {
                Original = fromMedia,
                Target = toMedia,
                OverwriteFiles = _classificationSettings.OverwriteFiles
            };
            
            var processor = _processorChainFactory.Build(fromMedia.Type);
            
            processor!.Process(request);
        }
    }

    private Media CreateClassificationMedia(Media fromMedia)
    { 
        var classificationFolderName = GetClassificationFolderName(fromMedia.Type);
        
        var createdDateTimeOffsetAsString = fromMedia.CreatedDateTime.ToString("yyyy-MM-dd");

        var newMediaFolder = _ioWrapper.Combine(new[] { fromMedia.DirectoryPath, classificationFolderName, createdDateTimeOffsetAsString });

        var newName = GetNewName(fromMedia.Type, fromMedia.Name, fromMedia.CreatedDateTime);

        var newNameWithoutExtension = _ioWrapper.GetFileNameWithoutExtension(newName);

        var newMediaPath = _ioWrapper.Combine(new[] { newMediaFolder, newName });

        var classificationMedia = new Media
        {
            Type = fromMedia.Type,
            Path = newMediaPath,
            DirectoryPath = newMediaFolder,
            Name = newName,
            NameWithoutExtension = newNameWithoutExtension,
            CreatedDateTime = fromMedia.CreatedDateTime,
        };
        
        return classificationMedia;
    }
    
    private string GetClassificationFolderName(MediaType mediaType)
    {
        return mediaType switch
        {
            MediaType.Image => _classificationSettings.Folders.ImageFolderName,
            MediaType.Raw => _classificationSettings.Folders.ImageFolderName,
            MediaType.Video => _classificationSettings.Folders.VideoFolderName,
            MediaType.WhatsAppImage => _classificationSettings.Folders.ImageFolderName,
            MediaType.WhatsAppVideo => _classificationSettings.Folders.VideoFolderName,
            MediaType.Insv => _classificationSettings.Folders.VideoFolderName,
            _ => throw new FormatException($"The provided media type '{mediaType}' is not supported."),
        };
    }

    private string GetNewName(MediaType mediaType, string originalName, DateTimeOffset createdDateTimeOffset)
    {
        if (!_classificationSettings.RenameMediaFiles)
        {
            return originalName;
        }

        var renameStrategy = _renameStrategyFactory.GetRenameStrategy(mediaType);

        var request = new RenameMediaRequest
        {
            Name = originalName,
            CreatedDate = createdDateTimeOffset,
            ReplaceName = _classificationSettings.ReplaceLongNames,
            MaximumNameLength = _classificationSettings.MaxMediaNameLength,
            NewName = _classificationSettings.NewMediaName
        };
        
        return renameStrategy.Rename(request);
    }
}

internal static partial class Log
{
    [LoggerMessage(Level = LogLevel.Information, Message = "Classification started")]
    public static partial void StartClassification(this ILogger<ClassificationService> logger);
    
    [LoggerMessage(Level = LogLevel.Information, Message = "Classification ended")]
    public static partial void EndClassification(this ILogger<ClassificationService> logger);
}