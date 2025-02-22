using System;
using OrderMedia.Interfaces;

namespace OrderMedia.Strategies.RenameStrategy;

public class Insta360RenameStrategy : IRenameStrategy
{
    private readonly IIOService _ioService;
    private readonly IRandomizerService _randomizerService;
    private readonly IConfigurationService _configurationService;

    public Insta360RenameStrategy(IIOService ioService, IRandomizerService randomizerService, IConfigurationService configurationService)
    {
        _ioService = ioService;
        _randomizerService = randomizerService;
        _configurationService = configurationService;
    }

    public string Rename(string name, DateTimeOffset createdDateTimeOffset)
    {
        var extension = _ioService.GetExtension(name);
        
        var cleanedName = GetCleanedName(name);
        
        var nameSplit = name.Split("_");

        var date = createdDateTimeOffset.ToString("yyyy-MM-dd_HH-mm-ss");

        var mediaName = string.Empty;
        
        if (ReplaceName(cleanedName.Length))
        {
            var randomNumber = _randomizerService.GetRandomNumberAsD4();
            mediaName = $"{_configurationService.GetNewMediaName()}_{randomNumber}";
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
        var nameWithoutExtension = _ioService.GetFileNameWithoutExtension(name);

        var cleanedNameSplit = nameWithoutExtension.Split("_");

        return cleanedNameSplit[4];
    }
    
    private bool ReplaceName(int cleanedNameLength) {
        return cleanedNameLength > _configurationService.GetMaxMediaNameLength() && _configurationService.GetReplaceLongNames();
    }
}