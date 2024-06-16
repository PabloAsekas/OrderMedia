using System;
using OrderMedia.Interfaces;
using OrderMedia.Models;

namespace OrderMedia.Handlers.Processor;

public class MoveMediaProcessorHandler : BaseProcessorHandler
{
    private readonly IIOService _ioService;

    public MoveMediaProcessorHandler(IIOService ioService)
    {
        _ioService = ioService;
    }

    public override void Process(Media media)
    {
        if (media.CreatedDateTime == default(DateTime))
        {
            return;
        }

        _ioService.CreateFolder(media.NewMediaFolder);

        try
        {
            _ioService.MoveMedia(media.MediaPath, media.NewMediaPath);
            base.Process(media);
        }
        catch (Exception e)
        {
            return;
        }
    }
}