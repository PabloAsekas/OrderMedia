using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OrderMedia.Interfaces;
using OrderMedia.Configuration;

namespace OrderMedia.ConsoleApp.Services;

public class CopyAaeFilesService
{
    private readonly ILogger<OrderMediaService> _logger;
    private readonly IIoWrapper _ioWrapper;
    private readonly ICopyComplementFilesService _copyComplementFilesService;
    private readonly MediaExtensionsSettings _mediaExtensionsSettings;
    private readonly MediaPathsSettings _mediaPathsSettings;

    public CopyAaeFilesService(
        ILogger<OrderMediaService> logger,
        IIoWrapper ioWrapper,
        ICopyComplementFilesService copyComplementFilesService,
        IOptions<MediaExtensionsSettings> mediaExtensionsOptions,
        IOptions<MediaPathsSettings> mediaPathsOptions)
    {
        _logger = logger;
        _ioWrapper = ioWrapper;
        _copyComplementFilesService = copyComplementFilesService;
        _mediaExtensionsSettings = mediaExtensionsOptions.Value;
        _mediaPathsSettings = mediaPathsOptions.Value;
    }
    
    public void Run()
    {
        var heicExtensions = _mediaExtensionsSettings.ImageExtensions.Where(e => e == ".heic").ToArray<string>();
        var heicPhotos = _ioWrapper.GetFilesByExtensions(_mediaPathsSettings.MediaPostProcessPath, heicExtensions);

        foreach (var photo in heicPhotos)
        {
            _copyComplementFilesService.CopyComplementFiles(photo.Name, ".aae");
        }
    }
}