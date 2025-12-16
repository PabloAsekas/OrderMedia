using Microsoft.Extensions.Options;
using OrderMedia.Interfaces;
using OrderMedia.Models;
using OrderMedia.Configuration;

namespace OrderMedia.Handlers.Processor;

public class MoveXmpProcessorHandler : BaseProcessorHandler
{
    private readonly IIoWrapper _ioWrapper;

    public MoveXmpProcessorHandler(IIoWrapper ioWrapper)
    {
        _ioWrapper = ioWrapper;
    }
    
    public override void Process(ProcessMediaRequest request)
    {
        var xmpName = $"{request.Original.NameWithoutExtension}.xmp";
        var xmpLocation = _ioWrapper.Combine([
            request.Original.DirectoryPath,
            xmpName
        ]);

        if (_ioWrapper.FileExists(xmpLocation))
        {
            var newXmpName = $"{request.Target.NameWithoutExtension}.xmp";
            var newXmpLocation = _ioWrapper.Combine([
                request.Target.DirectoryPath,
                newXmpName
            ]);

            _ioWrapper.MoveMedia(xmpLocation, newXmpLocation, request.OverwriteFiles);
        }

        base.Process(request);
    }
}