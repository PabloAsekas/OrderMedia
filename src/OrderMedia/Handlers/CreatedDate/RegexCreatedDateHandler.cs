using System.Text.RegularExpressions;
using OrderMedia.Interfaces;
using OrderMedia.Models;

namespace OrderMedia.Handlers.CreatedDate;

public class RegexCreatedDateHandler : BaseCreatedDateHandler
{
    private readonly IIoWrapper _ioWrapper;
    private readonly string _pattern;
    private readonly string _format;

    public RegexCreatedDateHandler(IIoWrapper ioWrapper, string pattern, string format)
    {
        _ioWrapper = ioWrapper;
        _pattern = pattern;
        _format = format;
    }

    public override CreatedDateInfo? GetCreatedDateInfo(string mediaPath)
    {
        var name = _ioWrapper.GetFileNameWithoutExtension(mediaPath);
        
        var m = Regex.Match(name, _pattern, RegexOptions.IgnoreCase);

        var createdDateInfo = CreateCreatedDateInfo(m.Value, _format);
        
        return createdDateInfo ?? base.GetCreatedDateInfo(mediaPath);
    }
}