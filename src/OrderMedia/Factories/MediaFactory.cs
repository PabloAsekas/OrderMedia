using System;
using OrderMedia.Enums;
using OrderMedia.Interfaces;
using OrderMedia.Interfaces.Factories;
using OrderMedia.Models;

namespace OrderMedia.Factories;

/// <summary>
/// Media factory to create media objects based on the path.
/// </summary>
public class MediaFactory : IMediaFactory
{
    private readonly IConfigurationService _configurationService;
    private readonly IIOService _ioService;
    private readonly IRenameStrategyFactory _renameStrategyFactory;
    private readonly IMediaTypeService _mediaTypeService;
    private readonly ICreatedDateExtractorService _createdDateExtractorService;

    public MediaFactory(IConfigurationService configurationService,
        IIOService ioService,
        IRenameStrategyFactory renameStrategyFactory,
        IMediaTypeService mediaTypeService,
        ICreatedDateExtractorService createdDateExtractorService)
    {
        _configurationService = configurationService;
        _ioService = ioService;
        _renameStrategyFactory = renameStrategyFactory;
        _mediaTypeService = mediaTypeService;
        _createdDateExtractorService = createdDateExtractorService;
    }

    public Media CreateMedia(string path)
    {
        var mediaType = _mediaTypeService.GetMediaType(path);

        var classificationFolderName = GetClassificationFolderName(mediaType);

        var mediaFolder = _ioService.GetDirectoryName(path);

        var fullName = _ioService.GetFileName(path);

        var nameWithoutExtension = _ioService.GetFileNameWithoutExtension(path);
            
        var createdDateTimeOffset = _createdDateExtractorService.GetCreatedDateTimeOffset(path);

        var createdDateTimeOffsetAsString = createdDateTimeOffset.ToString("yyyy-MM-dd");

        var newMediaFolder = _ioService.Combine(new string[] { mediaFolder, classificationFolderName, createdDateTimeOffsetAsString });

        var newName = GetNewName(mediaType, fullName, createdDateTimeOffset);

        var newNameWithoutExtension = _ioService.GetFileNameWithoutExtension(newName);

        var newMediaPath = _ioService.Combine(new string[] { newMediaFolder, newName });

        return new Media()
        {
            MediaType = mediaType,
            MediaPath = path,
            MediaFolder = mediaFolder,
            Name = fullName,
            NameWithoutExtension = nameWithoutExtension,
            CreatedDateTimeOffset = createdDateTimeOffset,
            NewMediaPath = newMediaPath,
            NewMediaFolder = newMediaFolder,
            NewName = newName,
            NewNameWithoutExtension = newNameWithoutExtension,
        };
    }

    private string GetClassificationFolderName(MediaType mediaType)
    {
        return mediaType switch
        {
            MediaType.Image => _configurationService.GetImageFolderName(),
            MediaType.Raw => _configurationService.GetImageFolderName(),
            MediaType.Video => _configurationService.GetVideoFolderName(),
            MediaType.WhatsAppImage => _configurationService.GetImageFolderName(),
            MediaType.WhatsAppVideo => _configurationService.GetVideoFolderName(),
            MediaType.Insv => _configurationService.GetVideoFolderName(),
            _ => throw new FormatException($"The provided media type '{mediaType}' is not supported."),
        };
    }

    private string GetNewName(MediaType mediaType, string originalName, DateTimeOffset createdDateTimeOffset)
    {
        if (!_configurationService.GetRenameMediaFiles())
        {
            return originalName;
        }

        var renameStrategy = _renameStrategyFactory.GetRenameStrategy(mediaType);
            
        return renameStrategy.Rename(originalName, createdDateTimeOffset);
    }
}