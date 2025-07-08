using System;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Options;
using OrderMedia.Interfaces;
using OrderMedia.Configuration;

namespace OrderMedia.Strategies.RenameStrategy;

public class DefaultRenameStrategy : IRenameStrategy
{
    private readonly IIoWrapper _ioWrapper;
    private readonly IRandomizerService _randomizerService;
    private readonly ClassificationSettingsOptions _classificationSettingsOptions;

    public DefaultRenameStrategy(
        IIoWrapper ioWrapper,
        IRandomizerService randomizerService,
        IOptions<ClassificationSettingsOptions> classificationSettingsOptions
        )
    {
        _ioWrapper = ioWrapper;
        _randomizerService = randomizerService;
        _classificationSettingsOptions = classificationSettingsOptions.Value;
    }

    public string Rename(string name, DateTimeOffset createdDateTimeOffset)
    {
        var finalName = createdDateTimeOffset.ToString("yyyy-MM-dd_HH-mm-ss");

        var extension = _ioWrapper.GetExtension(name);

        var cleanedName = GetCleanedName(name);

        if (ReplaceName(cleanedName.Length))
        {
            var randomNumber = _randomizerService.GetRandomNumberAsD4();
            finalName += $"_{_classificationSettingsOptions.NewMediaName}_{randomNumber}";
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
        var cleanedName = _ioWrapper.GetFileNameWithoutExtension(name);

        // Remove possible (1), (2), etc. from the name.
        cleanedName = Regex.Replace(cleanedName, @"\([\d]\)", string.Empty);

        // Remove possible start and end spaces.
        cleanedName = cleanedName.Trim();

        return cleanedName;
    }

    private bool ReplaceName(int cleanedNameLength) {
        return cleanedNameLength > _classificationSettingsOptions.MaxMediaNameLength && _classificationSettingsOptions.ReplaceLongNames;
    }
}