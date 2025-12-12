using System;
using Microsoft.Extensions.Options;
using OrderMedia.Interfaces;
using OrderMedia.Models;
using OrderMedia.Configuration;

namespace OrderMedia.Handlers.Processor;

public class MoveMediaProcessorHandler : BaseProcessorHandler
{
    private readonly IIoWrapper _ioWrapper;
    private readonly ClassificationSettings  _classificationSettings;

    public MoveMediaProcessorHandler(
        IIoWrapper ioWrapper,
        IOptions<ClassificationSettings> classificationSettingsOptions)
    {
        _ioWrapper = ioWrapper;
        _classificationSettings = classificationSettingsOptions.Value;
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
            _ioWrapper.MoveMedia(media.MediaPath, media.NewMediaPath, _classificationSettings.OverwriteFiles);
            base.Process(media);
        }
        catch (Exception e)
        {
            return;
        }
    }
}