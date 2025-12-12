using System;
using Microsoft.Extensions.Options;
using OrderMedia.Enums;
using OrderMedia.Interfaces;
using OrderMedia.Interfaces.Factories;
using OrderMedia.Models;
using OrderMedia.Configuration;

namespace OrderMedia.Factories;

/// <summary>
/// Media factory to create media objects based on the path.
/// </summary>
public class MediaFactory : IMediaFactory
{
    private readonly IIoWrapper _ioWrapper;
    private readonly IRenameStrategyFactory _renameStrategyFactory;
    private readonly IMediaTypeService _mediaTypeService;
    private readonly ICreatedDateExtractorService _createdDateExtractorService;
    private readonly ClassificationSettings _classificationSettings;

    public MediaFactory(
        IIoWrapper ioWrapper,
        IRenameStrategyFactory renameStrategyFactory,
        IMediaTypeService mediaTypeService,
        ICreatedDateExtractorService createdDateExtractorService,
        IOptions<ClassificationSettings> classificationSettingsOptions)
    {
        _ioWrapper = ioWrapper;
        _renameStrategyFactory = renameStrategyFactory;
        _mediaTypeService = mediaTypeService;
        _createdDateExtractorService = createdDateExtractorService;
        _classificationSettings = classificationSettingsOptions.Value;
    }

    public Media CreateMedia(string path)
    {
        var mediaType = _mediaTypeService.GetMediaType(path);

        var classificationFolderName = GetClassificationFolderName(mediaType);

        var mediaFolder = _ioWrapper.GetDirectoryName(path);

        var fullName = _ioWrapper.GetFileName(path);

        var nameWithoutExtension = _ioWrapper.GetFileNameWithoutExtension(path);
            
        var createdDateTimeOffset = _createdDateExtractorService.GetCreatedDateTimeOffset(path);

        var createdDateTimeOffsetAsString = createdDateTimeOffset.ToString("yyyy-MM-dd");

        var newMediaFolder = _ioWrapper.Combine(new[] { mediaFolder, classificationFolderName, createdDateTimeOffsetAsString });

        var newName = GetNewName(mediaType, fullName, createdDateTimeOffset);

        var newNameWithoutExtension = _ioWrapper.GetFileNameWithoutExtension(newName);

        var newMediaPath = _ioWrapper.Combine(new[] { newMediaFolder, newName });

        return new Media
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
            
        return renameStrategy.Rename(originalName, createdDateTimeOffset);
    }
}