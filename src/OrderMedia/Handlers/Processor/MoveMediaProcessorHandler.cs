using System;
using Microsoft.Extensions.Options;
using OrderMedia.Interfaces;
using OrderMedia.Models;
using OrderMedia.Configuration;

namespace OrderMedia.Handlers.Processor;

public class MoveMediaProcessorHandler : BaseProcessorHandler
{
    private readonly IIoWrapper _ioWrapper;

    public MoveMediaProcessorHandler(IIoWrapper ioWrapper)
    {
        _ioWrapper = ioWrapper;
    }

    public override void Process(ProcessMediaRequest request)
    {
        if (request.Original.CreatedDateTime == default(DateTimeOffset))
        {
            return;
        }

        _ioWrapper.CreateFolder(request.Target.DirectoryPath);

        try
        {
            _ioWrapper.MoveMedia(request.Original.Path, request.Target.Path, request.OverwriteFiles);
            
            base.Process(request);
        }
        catch (Exception e)
        {
            return;
        }
    }
}