using OrderMedia.Interfaces;
using OrderMedia.Models;

namespace OrderMedia.Handlers.Processor;

public class MoveM01XmlProcessorHandler : BaseProcessorHandler
{
    private readonly IIOService _ioService;

    public MoveM01XmlProcessorHandler(IIOService ioService)
    {
        _ioService = ioService;
    }

    public override void Process(Media media)
    {
        var m01XmlName = $"{media.NameWithoutExtension}M01.XML";
        var m01XmlLocation = _ioService.Combine(new string[] { media.MediaFolder, m01XmlName });

        if (_ioService.FileExists(m01XmlLocation))
        {
            var newM01XmlName = $"{media.NewNameWithoutExtension}M01.XML";
            var newM01XmlLocation = _ioService.Combine(new string[] { media.NewMediaFolder, newM01XmlName });

            _ioService.MoveMedia(m01XmlLocation, newM01XmlLocation);
        }

        base.Process(media);
    }
}