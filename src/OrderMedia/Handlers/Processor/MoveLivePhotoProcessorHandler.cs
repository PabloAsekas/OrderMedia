using System.Collections.Generic;
using Microsoft.Extensions.Options;
using OrderMedia.Interfaces;
using OrderMedia.Models;
using OrderMedia.Configuration;

namespace OrderMedia.Handlers.Processor;

public class MoveLivePhotoProcessorHandler : BaseProcessorHandler
{
    private readonly IIoWrapper _ioWrapper;
    private readonly ClassificationSettings _classificationSettings;

    public MoveLivePhotoProcessorHandler(
        IIoWrapper ioWrapper,
        IOptions<ClassificationSettings> classificationSettingsOptions)
    {
        _ioWrapper = ioWrapper;
        _classificationSettings = classificationSettingsOptions.Value;
    }

    public override void Process(Media media)
    {
        var possibleNames = new List<string>()
        {
            $"{media.NameWithoutExtension}.mov",
            $"{media.NameWithoutExtension}.mp4"
        };

        foreach (var videoName in possibleNames)
        {
            var result = FindAndMove(videoName, media);

            if (result)
                break;
        }

        base.Process(media);
    }
    
    private bool FindAndMove(string videoName, Media media)
    {
        var videoLocation = _ioWrapper.Combine([
            media.MediaFolder,
            videoName
        ]);

        var extension = _ioWrapper.GetExtension(videoName);

        if (!_ioWrapper.FileExists(videoLocation))
        {
            return false;
        }
        
        var newVideoName = $"{media.NewNameWithoutExtension}{extension}";
        var newVideoLocation = _ioWrapper.Combine([
            media.NewMediaFolder,
            newVideoName
        ]);

        _ioWrapper.MoveMedia(videoLocation, newVideoLocation, _classificationSettings.OverwriteFiles);

        return true;

    }
}