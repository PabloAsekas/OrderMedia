using Microsoft.Extensions.Options;
using OrderMedia.Interfaces;
using OrderMedia.Models;
using OrderMedia.Configuration;

namespace OrderMedia.Handlers.Processor;

public class MoveXmpProcessorHandler : BaseProcessorHandler
{
    private readonly IIoWrapper _ioWrapper;
    private readonly ClassificationSettingsOptions  _classificationSettingsOptions;

    public MoveXmpProcessorHandler(
        IIoWrapper ioWrapper,
        IOptions<ClassificationSettingsOptions> classificationSettingsOptions)
    {
        _ioWrapper = ioWrapper;
        _classificationSettingsOptions = classificationSettingsOptions.Value;
    }
    
    public override void Process(Media media)
    {
        var xmpName = $"{media.NameWithoutExtension}.xmp";
        var xmpLocation = _ioWrapper.Combine([
            media.MediaFolder,
            xmpName
        ]);

        if (_ioWrapper.FileExists(xmpLocation))
        {
            var newXmpName = $"{media.NewNameWithoutExtension}.xmp";
            var newXmpLocation = _ioWrapper.Combine([
                media.NewMediaFolder,
                newXmpName
            ]);

            _ioWrapper.MoveMedia(xmpLocation, newXmpLocation, _classificationSettingsOptions.OverwriteFiles);
        }

        base.Process(media);
    }
}