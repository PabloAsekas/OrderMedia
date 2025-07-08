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
    private readonly ClassificationFoldersOptions _classificationFoldersOptions;
    private readonly ClassificationSettingsOptions _classificationSettingsOptions;

    public MediaFactory(
        IIoWrapper ioWrapper,
        IRenameStrategyFactory renameStrategyFactory,
        IMediaTypeService mediaTypeService,
        ICreatedDateExtractorService createdDateExtractorService,
        IOptions<ClassificationFoldersOptions> classificationFoldersOptions,
        IOptions<ClassificationSettingsOptions> classificationSettingsOptions)
    {
        _ioWrapper = ioWrapper;
        _renameStrategyFactory = renameStrategyFactory;
        _mediaTypeService = mediaTypeService;
        _createdDateExtractorService = createdDateExtractorService;
        _classificationFoldersOptions = classificationFoldersOptions.Value;
        _classificationSettingsOptions = classificationSettingsOptions.Value;
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

        var newMediaFolder = _ioWrapper.Combine(new string[] { mediaFolder, classificationFolderName, createdDateTimeOffsetAsString });

        var newName = GetNewName(mediaType, fullName, createdDateTimeOffset);

        var newNameWithoutExtension = _ioWrapper.GetFileNameWithoutExtension(newName);

        var newMediaPath = _ioWrapper.Combine(new string[] { newMediaFolder, newName });

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
            MediaType.Image => _classificationFoldersOptions.ImageFolderName,
            MediaType.Raw => _classificationFoldersOptions.ImageFolderName,
            MediaType.Video => _classificationFoldersOptions.VideoFolderName,
            MediaType.WhatsAppImage => _classificationFoldersOptions.ImageFolderName,
            MediaType.WhatsAppVideo => _classificationFoldersOptions.VideoFolderName,
            MediaType.Insv => _classificationFoldersOptions.VideoFolderName,
            _ => throw new FormatException($"The provided media type '{mediaType}' is not supported."),
        };
    }

    private string GetNewName(MediaType mediaType, string originalName, DateTimeOffset createdDateTimeOffset)
    {
        if (!_classificationSettingsOptions.RenameMediaFiles)
        {
            return originalName;
        }

        var renameStrategy = _renameStrategyFactory.GetRenameStrategy(mediaType);
            
        return renameStrategy.Rename(originalName, createdDateTimeOffset);
    }
}