using Microsoft.Extensions.Options;
using OrderMedia.Interfaces;
using OrderMedia.Models;
using OrderMedia.Configuration;

namespace OrderMedia.Handlers.Processor;

public class MoveM01XmlProcessorHandler : BaseProcessorHandler
{
    private readonly IIoWrapper _ioWrapper;

    public MoveM01XmlProcessorHandler(IIoWrapper ioWrapper)
    {
        _ioWrapper = ioWrapper;
    }

    public override void Process(ProcessMediaRequest request)
    {
        var m01XmlName = $"{request.Original.NameWithoutExtension}M01.XML";
        var m01XmlLocation = _ioWrapper.Combine([
            request.Original.DirectoryPath,
            m01XmlName
        ]);

        if (_ioWrapper.FileExists(m01XmlLocation))
        {
            var newM01XmlName = $"{request.Target.NameWithoutExtension}M01.XML";
            var newM01XmlLocation = _ioWrapper.Combine([
                request.Target.DirectoryPath,
                newM01XmlName
            ]);

            _ioWrapper.MoveMedia(m01XmlLocation, newM01XmlLocation, request.OverwriteFiles);
        }

        base.Process(request);
    }
}