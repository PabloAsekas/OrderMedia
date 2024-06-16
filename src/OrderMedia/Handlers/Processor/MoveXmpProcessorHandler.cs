using OrderMedia.Interfaces;
using OrderMedia.Models;

namespace OrderMedia.Handlers.Processor;

public class MoveXmpProcessorHandler : BaseProcessorHandler
{
    private readonly IIOService _ioService;

    public MoveXmpProcessorHandler(IIOService ioService)
    {
        _ioService = ioService;
    }
    
    public override void Process(Media media)
    {
        var xmpName = $"{media.NameWithoutExtension}.xmp";
        var xmpLocation = _ioService.Combine(new string[] { media.MediaFolder, xmpName });

        if (_ioService.FileExists(xmpLocation))
        {
            var newXmpName = $"{media.NewNameWithoutExtension}.xmp";
            var newXmpLocation = _ioService.Combine(new string[] { media.NewMediaFolder, newXmpName });

            _ioService.MoveMedia(xmpLocation, newXmpLocation);
        }

        base.Process(media);
    }
}