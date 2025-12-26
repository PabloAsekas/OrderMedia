using System.Collections.Generic;
using OrderMedia.Interfaces;
using OrderMedia.Models;

namespace OrderMedia.Handlers.Processor;

public class MoveLivePhotoProcessorHandler : BaseProcessorHandler
{
    private readonly IIoWrapper _ioWrapper;

    public MoveLivePhotoProcessorHandler(IIoWrapper ioWrapper)
    {
        _ioWrapper = ioWrapper;
    }

    public override void Process(ProcessMediaRequest request)
    {
        var possibleNames = new List<string>()
        {
            $"{request.Original.NameWithoutExtension}.mov",
            $"{request.Original.NameWithoutExtension}.mp4"
        };

        foreach (var videoName in possibleNames)
        {
            var result = FindAndMove(videoName, request);

            if (result)
                break;
        }

        base.Process(request);
    }
    
    private bool FindAndMove(string videoName, ProcessMediaRequest request)
    {
        var videoLocation = _ioWrapper.Combine([
            request.Original.DirectoryPath,
            videoName
        ]);

        var extension = _ioWrapper.GetExtension(videoName);

        if (!_ioWrapper.FileExists(videoLocation))
        {
            return false;
        }
        
        var newVideoName = $"{request.Target.NameWithoutExtension}{extension}";
        var newVideoLocation = _ioWrapper.Combine([
            request.Target.DirectoryPath,
            newVideoName
        ]);

        _ioWrapper.MoveMedia(videoLocation, newVideoLocation, request.OverwriteFiles);

        return true;

    }
}