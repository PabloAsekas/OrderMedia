using Microsoft.Extensions.Options;
using OrderMedia.ConsoleApp.Configuration;
using OrderMedia.ConsoleApp.Interfaces;
using OrderMedia.Interfaces;
using OrderMedia.Interfaces.Factories;
using OrderMedia.Models;

namespace OrderMedia.ConsoleApp.Services;

public class RenamingService : IRenamingService
{
    private readonly IRenameStrategyFactory _renameStrategyFactory;
    private readonly RenamingSettings _renamingSettings;
    private readonly IIoWrapper _ioWrapper;

    public RenamingService(
        IRenameStrategyFactory renameStrategyFactory,
        IOptions<RenamingSettings> renamingSettings,
        IIoWrapper ioWrapper)
    {
        _renameStrategyFactory = renameStrategyFactory;
        _renamingSettings = renamingSettings.Value;
        _ioWrapper = ioWrapper;
    }

    public Media Rename(Media original)
    {
        var targetName = ResolveTargetName(original);

        return CreateTargetMedia(original, targetName);
    }

    private string ResolveTargetName(Media original)
    {
        var strategy = _renameStrategyFactory.GetRenameStrategy(original.Type);

        var request = new RenameMediaRequest
        {
            Name = original.Name,
            CreatedDate =  original.CreatedDateTime,
            ReplaceName = _renamingSettings.ReplaceLongNames,
            MaximumNameLength = _renamingSettings.MaxMediaNameLength,
            NewName = _renamingSettings.NewMediaName
        };
        
        return strategy.Rename(request);
    }

    private Media CreateTargetMedia(Media original, string targetName)
    {
        var path = _ioWrapper.Combine([original.DirectoryPath, targetName]);
        var nameWithoutExtension = _ioWrapper.GetFileNameWithoutExtension(targetName);

        return new Media
        {
            Type = original.Type,
            Path = path,
            DirectoryPath = original.DirectoryPath,
            Name = targetName,
            NameWithoutExtension = nameWithoutExtension,
            CreatedDateTime = original.CreatedDateTime
        };
    }
}