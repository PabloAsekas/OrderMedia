using System.Text.RegularExpressions;
using OrderMedia.Interfaces;
using OrderMedia.Models;

namespace OrderMedia.Strategies.RenameStrategy;

public class DefaultRenameStrategy : IRenameStrategy
{
    private readonly IIoWrapper _ioWrapper;
    private readonly IRandomizerService _randomizerService;

    public DefaultRenameStrategy(
        IIoWrapper ioWrapper,
        IRandomizerService randomizerService)
    {
        _ioWrapper = ioWrapper;
        _randomizerService = randomizerService;
    }

    public string Rename(RenameMediaRequest request)
    {
        var finalName = request.CreatedDate.ToString("yyyy-MM-dd_HH-mm-ss");

        var extension = _ioWrapper.GetExtension(request.Name);

        var cleanedName = GetCleanedName(request.Name);

        if (ReplaceName(request.ReplaceName, request.MaximumNameLength, cleanedName.Length))
        {
            var randomNumber = _randomizerService.GetRandomNumberAsD4();
            finalName += $"_{request.NewName}_{randomNumber}";
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
    
    private bool ReplaceName(bool replaceName, int maximumNameLength, int nameLength) {
        return replaceName && nameLength > maximumNameLength;
    }
}