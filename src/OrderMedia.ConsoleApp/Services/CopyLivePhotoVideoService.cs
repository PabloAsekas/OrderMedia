using Microsoft.Extensions.Options;
using OrderMedia.Interfaces;
using OrderMedia.Configuration;

namespace OrderMedia.ConsoleApp.Services;

public class CopyLivePhotoVideoService
{
    private readonly IIoWrapper _ioWrapper;
    private readonly ICopyComplementFilesService _copyComplementFilesService;
    private readonly MediaPathsOptions _mediaPathsOptions;
    private readonly MediaExtensionsOptions _mediaExtensionsOptions;
    
    public CopyLivePhotoVideoService(
        IIoWrapper ioWrapper,
        ICopyComplementFilesService copyComplementFilesService,
        IOptions<MediaPathsOptions> mediaPathsOptions,
        IOptions<MediaExtensionsOptions> mediaExtensionsOptions)
    {
        _ioWrapper = ioWrapper;
        _copyComplementFilesService = copyComplementFilesService;
        _mediaPathsOptions = mediaPathsOptions.Value;
        _mediaExtensionsOptions = mediaExtensionsOptions.Value;
    }

    public void Run()
    {
        var heicExtensions = _mediaExtensionsOptions.ImageExtensions.Where(e => e == ".heic").ToArray<string>();
        var heicPhotos = _ioWrapper.GetFilesByExtensions(_mediaPathsOptions.MediaPostProcessPath, heicExtensions);

        foreach (var photo in heicPhotos)
        {
            _copyComplementFilesService.CopyComplementFiles(photo.Name, ".mov");
        }
    }
}