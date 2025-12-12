using System.Collections.Generic;
using Microsoft.Extensions.Options;
using OrderMedia.Interfaces;
using OrderMedia.Models;
using OrderMedia.Configuration;

namespace OrderMedia.Handlers.Processor;

public class MoveAaeProcessorHandler : BaseProcessorHandler
{
    private readonly IIoWrapper _ioWrapper;
    private readonly IAaeHelperService _aaeHelperService;
    private readonly ClassificationSettings _classificationSettings;

    public MoveAaeProcessorHandler(
        IIoWrapper ioWrapper,
        IAaeHelperService aaeHelperService,
        IOptions<ClassificationSettings> classificationSettingsOptions)
    {
        _ioWrapper = ioWrapper;
        _aaeHelperService = aaeHelperService;
        _classificationSettings = classificationSettingsOptions.Value;
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
        var aaeLocation = _ioWrapper.Combine([
            media.MediaFolder,
            aaeName
        ]);

        if (!_ioWrapper.FileExists(aaeLocation))
        {
            return false;
        }
        
        var newAaeName = $"{media.NewNameWithoutExtension}.aae";
        var newAaeLocation = _ioWrapper.Combine([
            media.NewMediaFolder,
            newAaeName
        ]);

        _ioWrapper.MoveMedia(aaeLocation, newAaeLocation, _classificationSettings.OverwriteFiles);

        return true;
    }
}