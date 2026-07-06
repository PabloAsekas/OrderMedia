using OrderMedia.Interfaces;
using OrderMedia.Interfaces.Handlers;
using OrderMedia.Models;

namespace OrderMedia.Handlers.Sync;

public class RemoveSyncHandler : ISyncHandler<RemoveSyncRequest>
{
    private readonly IIoWrapper _ioWrapper;

    public RemoveSyncHandler(IIoWrapper ioWrapper)
    {
        _ioWrapper = ioWrapper;
    }

    public void Execute(RemoveSyncRequest request)
    {
        foreach (var relativePath in request.RelativePaths)
        {
            var destinationPath = string.Concat(request.DestinationPath, relativePath);
                
            var destinationFolder = _ioWrapper.GetDirectoryName(destinationPath);

            if (!_ioWrapper.FileExists(destinationPath))
            {
                continue;
            }

            _ioWrapper.DeleteFile(destinationPath);
                
            if (_ioWrapper.IsDirectoryEmpty(destinationFolder))
            {
                _ioWrapper.DeleteDirectory(destinationFolder);
            }
        }
    }
}