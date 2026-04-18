using System.Text.RegularExpressions;
using OrderMedia.ConsoleApp.Interfaces;
using OrderMedia.Models;

namespace OrderMedia.ConsoleApp.Services;

public class RenamingValidatorService : IRenamingValidatorService
{
    public bool ValidateMedia(Media media)
    {
        if (media.CreatedDateTime == default)
        {
            return false;
        }
        
        var m = Regex.Match(media.Name, "[0-9]{4}-(0[1-9]|1[0-2])-(0[1-9]|[1-2][0-9]|3[0-1])_(0[0-9]|[1-2][0-9])-([0-5][0-9])-([0-5][0-9])", RegexOptions.IgnoreCase);

        if (m.Success)
        {
            return false;
        }

        if (media.NameWithoutExtension.EndsWith("-HDR"))
        {
            return false;
        }
        
        if (media.NameWithoutExtension.Contains("-HDR-"))
        {
            return false;
        }
        
        if (media.NameWithoutExtension.EndsWith("-Pano"))
        {
            return false;
        }
        
        if (media.NameWithoutExtension.Contains("-Pano-"))
        {
            return false;
        }
        
        return true;
    }
}