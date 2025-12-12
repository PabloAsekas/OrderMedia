using Microsoft.Extensions.Options;
using OrderMedia.Interfaces;
using OrderMedia.Models;
using OrderMedia.Configuration;

namespace OrderMedia.Handlers.Processor;

public class MoveM01XmlProcessorHandler : BaseProcessorHandler
{
    private readonly IIoWrapper _ioWrapper;
    private readonly ClassificationSettings _classificationSettings;

    public MoveM01XmlProcessorHandler(
        IIoWrapper ioWrapper,
        IOptions<ClassificationSettings> classificationSettingsOptions)
    {
        _ioWrapper = ioWrapper;
        _classificationSettings = classificationSettingsOptions.Value;
    }

    public override void Process(Media media)
    {
        var m01XmlName = $"{media.NameWithoutExtension}M01.XML";
        var m01XmlLocation = _ioWrapper.Combine([
            media.MediaFolder,
            m01XmlName
        ]);

        if (_ioWrapper.FileExists(m01XmlLocation))
        {
            var newM01XmlName = $"{media.NewNameWithoutExtension}M01.XML";
            var newM01XmlLocation = _ioWrapper.Combine([
                media.NewMediaFolder,
                newM01XmlName
            ]);

            _ioWrapper.MoveMedia(m01XmlLocation, newM01XmlLocation, _classificationSettings.OverwriteFiles);
        }

        base.Process(media);
    }
}