using System.Text.RegularExpressions;
using OrderMedia.Interfaces;
using OrderMedia.Models;

namespace OrderMedia.Handlers.CreatedDate;

public class RegexCreatedDateHandler : BaseCreatedDateHandler
{
    private readonly IIOService _ioService;
    private readonly string _pattern;
    private readonly string _format;

    public RegexCreatedDateHandler(IIOService ioService, string pattern, string format)
    {
        _ioService = ioService;
        _pattern = pattern;
        _format = format;
    }

    public override CreatedDateInfo? GetCreatedDateInfo(string mediaPath)
    {
        var name = _ioService.GetFileNameWithoutExtension(mediaPath);
        
        var m = Regex.Match(name, _pattern, RegexOptions.IgnoreCase);

        var createdDateInfo = CreateCreatedDateInfo(m.Value, _format);
        
        return createdDateInfo ?? base.GetCreatedDateInfo(mediaPath);
    }
}