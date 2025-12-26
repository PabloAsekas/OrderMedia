using Microsoft.Extensions.Options;
using OrderMedia.Interfaces;
using OrderMedia.Interfaces.Factories;
using OrderMedia.ConsoleApp.Configuration;
using OrderMedia.ConsoleApp.Interfaces;
using OrderMedia.Models;

namespace OrderMedia.ConsoleApp.Services;

public class ClassificationService : IClassificationService
{
    private readonly IIoWrapper _ioWrapper;
    private readonly ClassificationSettings _classificationSettings;
    private readonly IRenameStrategyFactory _renameStrategyFactory;
    private readonly IClassificationMediaFolderStrategyResolver _mediaFolderStrategyResolver;

    public ClassificationService(
        IIoWrapper ioWrapper,
        IOptions<ClassificationSettings> classificationSettingsOptions,
        IRenameStrategyFactory renameStrategyFactory,
        IClassificationMediaFolderStrategyResolver mediaFolderStrategyResolver)
    {
        _ioWrapper = ioWrapper;
        _renameStrategyFactory = renameStrategyFactory;
        _classificationSettings = classificationSettingsOptions.Value;
        _mediaFolderStrategyResolver = mediaFolderStrategyResolver;
    }

    public Media Classify(Media original)
    {
        var targetFolder = ResolveTargetFolder(original);

        var targetDirectory = BuildTargetDirectory(original, targetFolder);

        var targetName = ResolveTargetName(original);

        return CreateTargetMedia(original, targetDirectory, targetName);
    }

    private string ResolveTargetFolder(Media original)
    {
        var strategy = _mediaFolderStrategyResolver.Resolve(original.Type);

        return strategy.GetTargetFolder();
    }

    private string BuildTargetDirectory(Media original, string targetFolder)
    {
        var folderName = original.CreatedDateTime.ToString("yyyy-MM-dd");
        
        return _ioWrapper.Combine([original.DirectoryPath, targetFolder, folderName]);
    }

    private string ResolveTargetName(Media original)
    {
        if (!_classificationSettings.RenameMediaFiles)
        {
            return original.Name;
        }
        
        var strategy = _renameStrategyFactory.GetRenameStrategy(original.Type);

        var request = new RenameMediaRequest
        {
            Name = original.Name,
            CreatedDate =  original.CreatedDateTime,
            ReplaceName = _classificationSettings.ReplaceLongNames,
            MaximumNameLength = _classificationSettings.MaxMediaNameLength,
            NewName = _classificationSettings.NewMediaName
        };
        
        return strategy.Rename(request);
    }

    private Media CreateTargetMedia(Media original, string targetDirectory, string targetName)
    {
        var path = _ioWrapper.Combine([targetDirectory, targetName]);
        var nameWithoutExtension = _ioWrapper.GetFileNameWithoutExtension(targetName);
        
        return new Media
        {
            Type = original.Type,
            Path = path,
            DirectoryPath = targetDirectory,
            Name = targetName,
            NameWithoutExtension = nameWithoutExtension,
            CreatedDateTime = original.CreatedDateTime,
        };
    }
}