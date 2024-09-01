using System.Collections.Generic;
using OrderMedia.Interfaces;
using OrderMedia.Models;

namespace OrderMedia.Handlers.Processor;

public class MoveAaeProcessorHandler : BaseProcessorHandler
{
    private readonly IIOService _ioService;
    private readonly IAaeHelperService _aaeHelperService;

    public MoveAaeProcessorHandler(IIOService ioService, IAaeHelperService aaeHelperService)
    {
        _ioService = ioService;
        _aaeHelperService = aaeHelperService;
    }
    
    public override void Process(Media media)
    {
        var possibleNames = new List<string>()
        {
            $"{media.NameWithoutExtension}.aae",
            _aaeHelperService.GetAaeName(media.NameWithoutExtension),
        };

        foreach (var aaeName in possibleNames)
        {
            var result = FindAndMove(aaeName, media);
            
            if (result)
            {
                break;
            }
        }
        
        base.Process(media);
    }
    
    private bool FindAndMove(string aaeName, Media media)
    {
        var aaeLocation = _ioService.Combine(new string[] { media.MediaFolder, aaeName });

        if (!_ioService.FileExists(aaeLocation))
        {
            return false;
        }
        
        var newAaeName = $"{media.NewNameWithoutExtension}.aae";
        var newAaeLocation = _ioService.Combine(new string[] { media.NewMediaFolder, newAaeName });

        _ioService.MoveMedia(aaeLocation, newAaeLocation);

        return true;
    }
}