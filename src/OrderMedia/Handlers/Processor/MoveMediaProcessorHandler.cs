using System;
using Microsoft.Extensions.Options;
using OrderMedia.Interfaces;
using OrderMedia.Models;
using OrderMedia.Configuration;

namespace OrderMedia.Handlers.Processor;

public class MoveMediaProcessorHandler : BaseProcessorHandler
{
    private readonly IIoWrapper _ioWrapper;
    private readonly ClassificationSettingsOptions  _classificationSettingsOptions;

    public MoveMediaProcessorHandler(
        IIoWrapper ioWrapper,
        IOptions<ClassificationSettingsOptions> classificationSettingsOptions)
    {
        _ioWrapper = ioWrapper;
        _classificationSettingsOptions = classificationSettingsOptions.Value;
    }

    public override void Process(Media media)
    {
        if (media.CreatedDateTimeOffset == default(DateTimeOffset))
        {
            return;
        }

        _ioWrapper.CreateFolder(media.NewMediaFolder);

        try
        {
            _ioWrapper.MoveMedia(media.MediaPath, media.NewMediaPath, _classificationSettingsOptions.OverwriteFiles);
            base.Process(media);
        }
        catch (Exception e)
        {
            return;
        }
    }
}