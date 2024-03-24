using OrderMedia.Interfaces;

namespace OrderMedia.ConsoleApp.Services;

public class CopyLivePhotoVideoService
{
    private readonly IIOService _ioService;
    private readonly IConfigurationService _configurationService;
    private readonly ICopyComplementFilesService _copyComplementFilesService;

    public CopyLivePhotoVideoService(IIOService ioService, IConfigurationService configurationService, ICopyComplementFilesService copyComplementFilesService)
    {
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
            _copyComplementFilesService.CopyComplementFiles(photo.Name, ".mov");
        }
    }
}