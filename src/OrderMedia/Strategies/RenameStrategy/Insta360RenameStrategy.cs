using OrderMedia.Interfaces;
using OrderMedia.Models;

namespace OrderMedia.Strategies.RenameStrategy;

public class Insta360RenameStrategy : IRenameStrategy
{
    private readonly IIoWrapper _ioWrapper;
    private readonly IRandomizerService _randomizerService;
    
    public Insta360RenameStrategy(
        IIoWrapper ioWrapper,
        IRandomizerService randomizerService)
    {
        _ioWrapper = ioWrapper;
        _randomizerService = randomizerService;
    }

    public string Rename(RenameMediaRequest request)
    {
        var extension = _ioWrapper.GetExtension(request.Name);
        
        var cleanedName = GetCleanedName(request.Name);
        
        var nameSplit = request.Name.Split("_");

        var date = request.CreatedDate.ToString("yyyy-MM-dd_HH-mm-ss");

        var mediaName = string.Empty;
        
        if (ReplaceName(request.ReplaceName, request.MaximumNameLength, cleanedName.Length))
        {
            var randomNumber = _randomizerService.GetRandomNumberAsD4();
            mediaName = $"{request.NewName}_{randomNumber}";
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
    
    private bool ReplaceName(bool replaceName, int maximumNameLength, int nameLength) {
        return replaceName && nameLength > maximumNameLength;
    }
}