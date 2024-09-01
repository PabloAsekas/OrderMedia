using System;
using System.Text.RegularExpressions;
using OrderMedia.Interfaces;

namespace OrderMedia.Strategies.RenameStrategy;

public class DefaultRenameStrategy : IRenameStrategy
{
    private readonly IIOService _ioService;
    private readonly IRandomizerService _randomizerService;
    private readonly IConfigurationService _configurationService;

    public DefaultRenameStrategy(IIOService ioService, IRandomizerService randomizerService, IConfigurationService configurationService)
    {
        _ioService = ioService;
        _randomizerService = randomizerService;
        _configurationService = configurationService;
    }

    public string Rename(string name, DateTime createdDateTime)
    {
        var finalName = createdDateTime.ToString("yyyy-MM-dd_HH-mm-ss");

        var extension = _ioService.GetExtension(name);

        var cleanedName = GetCleanedName(name);

        if (ReplaceName(cleanedName.Length))
        {
            var randomNumber = _randomizerService.GetRandomNumberAsD4();
            finalName += $"_{_configurationService.GetNewMediaName()}_{randomNumber}";
        }
        else
        {
            finalName += $"_{cleanedName}";
        }

        return $"{finalName}{extension}";
    }
    
    private string GetCleanedName(string name)
    {
        // Remove extension.
        var cleanedName = _ioService.GetFileNameWithoutExtension(name);

        // Remove possible (1), (2), etc. from the name.
        cleanedName = Regex.Replace(cleanedName, @"\([\d]\)", string.Empty);

        // Remove possible start and end spaces.
        cleanedName = cleanedName.Trim();

        return cleanedName;
    }

    private bool ReplaceName(int cleanedNameLength) {
        return cleanedNameLength > _configurationService.GetMaxMediaNameLength() && _configurationService.GetReplaceLongNames();
    }
}