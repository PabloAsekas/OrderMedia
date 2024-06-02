using System.Text.RegularExpressions;
using OrderMedia.Interfaces;
using OrderMedia.Models;

namespace OrderMedia.Handlers.CreatedDate;

public class WhatsAppCreatedDateHandler : BaseCreatedDateHandler
{
    private readonly IIOService _ioService;

    public WhatsAppCreatedDateHandler(IIOService ioService)
    {
        _ioService = ioService;
    }

    public override CreatedDateInfo GetCreatedDateInfo(string mediaPath)
    {
        var name = _ioService.GetFileNameWithoutExtension(mediaPath);
        
        const string pattern = @"[0-9]{4}-(0[1-9]|1[0-2])-(0[1-9]|[1-2][0-9]|3[0-1])-(0[0-9]|[1-2][0-9])-([0-5][0-9])-([0-5][0-9])";

        var m = Regex.Match(name, pattern, RegexOptions.IgnoreCase);

        var createdDateInfo = CreateCreatedDateInfo(m.Value, "yyyy-MM-dd-HH-mm-ss");
        
        return createdDateInfo ?? base.GetCreatedDateInfo(mediaPath);
    }
}