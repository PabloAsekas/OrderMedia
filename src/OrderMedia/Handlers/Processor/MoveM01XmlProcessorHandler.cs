using Microsoft.Extensions.Options;
using OrderMedia.Interfaces;
using OrderMedia.Models;
using OrderMedia.Configuration;

namespace OrderMedia.Handlers.Processor;

public class MoveM01XmlProcessorHandler : BaseProcessorHandler
{
    private readonly IIoWrapper _ioWrapper;
    private readonly ClassificationSettingsOptions _classificationSettingsOptions;

    public MoveM01XmlProcessorHandler(
        IIoWrapper ioWrapper,
        IOptions<ClassificationSettingsOptions> classificationSettingsOptions)
    {
        _ioWrapper = ioWrapper;
        _classificationSettingsOptions = classificationSettingsOptions.Value;
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

            _ioWrapper.MoveMedia(m01XmlLocation, newM01XmlLocation, _classificationSettingsOptions.OverwriteFiles);
        }

        base.Process(media);
    }
}