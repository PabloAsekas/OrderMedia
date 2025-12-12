using Microsoft.Extensions.Options;
using OrderMedia.Interfaces;
using OrderMedia.Models;
using OrderMedia.Configuration;

namespace OrderMedia.Handlers.Processor;

public class MoveXmpProcessorHandler : BaseProcessorHandler
{
    private readonly IIoWrapper _ioWrapper;
    private readonly ClassificationSettings  _classificationSettings;

    public MoveXmpProcessorHandler(
        IIoWrapper ioWrapper,
        IOptions<ClassificationSettings> classificationSettingsOptions)
    {
        _ioWrapper = ioWrapper;
        _classificationSettings = classificationSettingsOptions.Value;
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

            _ioWrapper.MoveMedia(xmpLocation, newXmpLocation, _classificationSettings.OverwriteFiles);
        }

        base.Process(media);
    }
}