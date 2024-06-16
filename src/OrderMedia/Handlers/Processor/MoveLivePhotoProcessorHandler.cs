using System.Collections.Generic;
using OrderMedia.Interfaces;
using OrderMedia.Models;

namespace OrderMedia.Handlers.Processor;

public class MoveLivePhotoProcessorHandler : BaseProcessorHandler
{
    private readonly IIOService _ioService;

    public MoveLivePhotoProcessorHandler(IIOService ioService)
    {
        _ioService = ioService;
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
        var videoLocation = _ioService.Combine(new string[] { media.MediaFolder, videoName });

        var extension = _ioService.GetExtension(videoName);

        if (!_ioService.FileExists(videoLocation))
        {
            return false;
        }
        
        var newVideoName = $"{media.NewNameWithoutExtension}{extension}";
        var newVideoLocation = _ioService.Combine(new string[] { media.NewMediaFolder, newVideoName });

        _ioService.MoveMedia(videoLocation, newVideoLocation);

        return true;

    }
}