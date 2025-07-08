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
    private readonly MediaExtensionsOptions _mediaExtensionsOptions;
    private readonly MediaPathsOptions _mediaPathsOptions;

    public CopyAaeFilesService(
        ILogger<OrderMediaService> logger,
        IIoWrapper ioWrapper,
        ICopyComplementFilesService copyComplementFilesService,
        IOptions<MediaExtensionsOptions> mediaExtensionsOptions,
        IOptions<MediaPathsOptions> mediaPathsOptions)
    {
        _logger = logger;
        _ioWrapper = ioWrapper;
        _copyComplementFilesService = copyComplementFilesService;
        _mediaExtensionsOptions = mediaExtensionsOptions.Value;
        _mediaPathsOptions = mediaPathsOptions.Value;
    }
    
    public void Run()
    {
        var heicExtensions = _mediaExtensionsOptions.ImageExtensions.Where(e => e == ".heic").ToArray<string>();
        var heicPhotos = _ioWrapper.GetFilesByExtensions(_mediaPathsOptions.MediaPostProcessPath, heicExtensions);

        foreach (var photo in heicPhotos)
        {
            _copyComplementFilesService.CopyComplementFiles(photo.Name, ".aae");
        }
    }
}