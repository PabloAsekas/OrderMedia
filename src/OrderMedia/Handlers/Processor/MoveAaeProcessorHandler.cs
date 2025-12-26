using System.Collections.Generic;
using OrderMedia.Interfaces;
using OrderMedia.Models;

namespace OrderMedia.Handlers.Processor;

public class MoveAaeProcessorHandler : BaseProcessorHandler
{
    private readonly IIoWrapper _ioWrapper;
    private readonly IAaeHelperService _aaeHelperService;

    public MoveAaeProcessorHandler(IIoWrapper ioWrapper, IAaeHelperService aaeHelperService)
    {
        _ioWrapper = ioWrapper;
        _aaeHelperService = aaeHelperService;
    }
    
    public override void Process(ProcessMediaRequest request)
    {
        var possibleNames = new List<string>
        {
            $"{request.Original.NameWithoutExtension}.aae",
            _aaeHelperService.GetAaeName(request.Original.NameWithoutExtension),
        };

        foreach (var aaeName in possibleNames)
        {
            var result = FindAndMove(aaeName, request);
            
            if (result)
            {
                break;
            }
        }
        
        base.Process(request);
    }
    
    private bool FindAndMove(string aaeName, ProcessMediaRequest request)
    {
        var aaeLocation = _ioWrapper.Combine([
            request.Original.DirectoryPath,
            aaeName
        ]);

        if (!_ioWrapper.FileExists(aaeLocation))
        {
            return false;
        }
        
        var newAaeName = $"{request.Target.NameWithoutExtension}.aae";
        var newAaeLocation = _ioWrapper.Combine([
            request.Target.DirectoryPath,
            newAaeName
        ]);

        _ioWrapper.MoveMedia(aaeLocation, newAaeLocation, request.OverwriteFiles);

        return true;
    }
}