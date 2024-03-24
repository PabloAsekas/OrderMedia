using Microsoft.Extensions.Logging;
using OrderMedia.Interfaces;

namespace OrderMedia.ConsoleApp.Services;

public class CopyAaeFilesService
{
    private readonly ILogger<OrderMediaService> _logger;
    private readonly IIOService _ioService;
    private readonly IConfigurationService _configurationService;
    private readonly ICopyComplementFilesService _copyComplementFilesService;

    public CopyAaeFilesService(ILogger<OrderMediaService> logger, IIOService ioService, IConfigurationService configurationService, ICopyComplementFilesService copyComplementFilesService)
    {
        _logger = logger;
        _ioService = ioService;
        _configurationService = configurationService;
        _copyComplementFilesService = copyComplementFilesService;
    }
    
    public void Run()
    {
        var heicExtensions = _configurationService.GetImageExtensions().Where(e => e == ".heic").ToArray<string>();
        var heicPhotos = _ioService.GetFilesByExtensions(_configurationService.GetMediaPostProcessPath(), heicExtensions);

        foreach (var photo in heicPhotos)
        {
            _copyComplementFilesService.CopyComplementFiles(photo.Name, ".aae");
        }
    }
}