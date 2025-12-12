using Microsoft.Extensions.Options;
using OrderMedia.Interfaces;
using OrderMedia.Configuration;

namespace OrderMedia.ConsoleApp.Services;

public class CopyLivePhotoVideoService
{
    private readonly IIoWrapper _ioWrapper;
    private readonly ICopyComplementFilesService _copyComplementFilesService;
    private readonly MediaPathsSettings _mediaPathsSettings;
    private readonly MediaExtensionsSettings _mediaExtensionsSettings;
    
    public CopyLivePhotoVideoService(
        IIoWrapper ioWrapper,
        ICopyComplementFilesService copyComplementFilesService,
        IOptions<MediaPathsSettings> mediaPathsOptions,
        IOptions<MediaExtensionsSettings> mediaExtensionsOptions)
    {
        _ioWrapper = ioWrapper;
        _copyComplementFilesService = copyComplementFilesService;
        _mediaPathsSettings = mediaPathsOptions.Value;
        _mediaExtensionsSettings = mediaExtensionsOptions.Value;
    }

    public void Run()
    {
        var heicExtensions = _mediaExtensionsSettings.ImageExtensions.Where(e => e == ".heic").ToArray<string>();
        var heicPhotos = _ioWrapper.GetFilesByExtensions(_mediaPathsSettings.MediaPostProcessPath, heicExtensions);

        foreach (var photo in heicPhotos)
        {
            _copyComplementFilesService.CopyComplementFiles(photo.Name, ".mov");
        }
    }
}