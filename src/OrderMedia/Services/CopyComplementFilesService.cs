using System.Linq;
using Microsoft.Extensions.Logging;
using OrderMedia.Interfaces;

namespace OrderMedia.Services;

public class CopyComplementFilesService : ICopyComplementFilesService
{
    private readonly IIOService _ioService;
    private readonly IConfigurationService _configurationService;
    private readonly ILogger<CopyComplementFilesService> _logger;

    public CopyComplementFilesService(IIOService ioService, IConfigurationService configurationService, ILogger<CopyComplementFilesService> logger)
    {
        _ioService = ioService;
        _configurationService = configurationService;
        _logger = logger;
    }

    public void CopyComplementFiles(string fileToApply, string extensionToSearch)
    {
        var name = _ioService.GetFileNameWithoutExtension(fileToApply);

        var date = name[..10];

        var year = date[..4];

        var yearFolder = _ioService.Combine(new string[] { _configurationService.GetMediaPostProcessSource(), year });
        
        if (!_ioService.DirectoryExists(yearFolder))
        {
            _logger.LogInformation("La carpeta no existe {0}", yearFolder);
            
            return;
        }

        var directories = _ioService.GetDirectories(yearFolder);

        var directoriesByDate = directories.Where(d => d.Contains(yearFolder + "/" + date));

        foreach (var folder in directoriesByDate)
        {
            var searchFile = folder + "/" + name + extensionToSearch;

            if (!_ioService.FileExists(searchFile))
            {
                continue;
            }
            
            var finalName = _configurationService.GetMediaPostProcessPath() + name + extensionToSearch;
                
            _ioService.CopyFile(searchFile, finalName);
        }
    }
}