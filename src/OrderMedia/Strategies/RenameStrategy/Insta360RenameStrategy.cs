using System;
using Microsoft.Extensions.Options;
using OrderMedia.Interfaces;
using OrderMedia.Configuration;

namespace OrderMedia.Strategies.RenameStrategy;

public class Insta360RenameStrategy : IRenameStrategy
{
    private readonly IIoWrapper _ioWrapper;
    private readonly IRandomizerService _randomizerService;
    private readonly ClassificationSettingsOptions _classificationSettingsOptions;
    
    public Insta360RenameStrategy(
        IIoWrapper ioWrapper,
        IRandomizerService randomizerService,
        IOptions<ClassificationSettingsOptions> classificationSettingsOptions)
    {
        _ioWrapper = ioWrapper;
        _randomizerService = randomizerService;
        _classificationSettingsOptions = classificationSettingsOptions.Value;
    }

    public string Rename(string name, DateTimeOffset createdDateTimeOffset)
    {
        var extension = _ioWrapper.GetExtension(name);
        
        var cleanedName = GetCleanedName(name);
        
        var nameSplit = name.Split("_");

        var date = createdDateTimeOffset.ToString("yyyy-MM-dd_HH-mm-ss");

        var mediaName = string.Empty;
        
        if (ReplaceName(cleanedName.Length))
        {
            var randomNumber = _randomizerService.GetRandomNumberAsD4();
            mediaName = $"{_classificationSettingsOptions.NewMediaName}_{randomNumber}";
        }
        else
        {
            mediaName += $"{cleanedName}";
        }
        
        return $"{nameSplit[0]}_{date}_{nameSplit[3]}_{mediaName}{extension}";
    }
    
    private string GetCleanedName(string name)
    {
        // Remove extension.
        var nameWithoutExtension = _ioWrapper.GetFileNameWithoutExtension(name);

        var cleanedNameSplit = nameWithoutExtension.Split("_");

        return cleanedNameSplit[4];
    }
    
    private bool ReplaceName(int cleanedNameLength) {
        return cleanedNameLength > _classificationSettingsOptions.MaxMediaNameLength && _classificationSettingsOptions.ReplaceLongNames;
    }
}