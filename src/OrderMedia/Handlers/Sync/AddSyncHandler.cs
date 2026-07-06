using OrderMedia.Interfaces;
using OrderMedia.Interfaces.Handlers;
using OrderMedia.Models;

namespace OrderMedia.Handlers.Sync;

public class AddSyncHandler : ISyncHandler<AddSyncRequest>
{
    private readonly IIoWrapper _ioWrapper;

    public AddSyncHandler(IIoWrapper ioWrapper)
    {
        _ioWrapper = ioWrapper;
    }

    public void Execute(AddSyncRequest request)
    {
        foreach (var relativePath in request.RelativePaths)
        {
            var sourcePath = _ioWrapper.Combine([request.SourcePath, relativePath]);

            if (!_ioWrapper.FileExists(sourcePath))
            {
                continue;
            }
        
            var destinationPath = _ioWrapper.Combine([request.DestinationPath, relativePath]);
        
            var destinationFolder = _ioWrapper.GetDirectoryName(destinationPath);

            if (!_ioWrapper.DirectoryExists(destinationFolder))
            {
                _ioWrapper.CreateFolder(destinationFolder);
            }
        
            _ioWrapper.CopyFile(sourcePath, destinationPath);
        }
    }
}