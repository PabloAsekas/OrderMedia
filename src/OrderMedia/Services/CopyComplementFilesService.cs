using Microsoft.Extensions.Logging;
using OrderMedia.Interfaces;

namespace OrderMedia.Services;

public class CopyComplementFilesService : ICopyComplementFilesService
{
    private readonly IIoWrapper _ioWrapper;
    private readonly ILogger<CopyComplementFilesService> _logger;
    
    public CopyComplementFilesService(
        IIoWrapper ioWrapper,
        ILogger<CopyComplementFilesService> logger)
    {
        _ioWrapper = ioWrapper;
        _logger = logger;
    }

    public void CopyComplementFiles(string fileToApply, string extensionToSearch)
    {
        var name = _ioWrapper.GetFileNameWithoutExtension(fileToApply);

        var date = name[..10];

        var year = date[..4];

        /*var yearFolder = _ioWrapper.Combine([
            _mediaPathsSettings.MediaPostProcessSource,
            year
        ]);
        
        if (!_ioWrapper.DirectoryExists(yearFolder))
        {
            _logger.LogInformation("La carpeta no existe {0}", yearFolder);
            
            return;
        }

        var directories = _ioWrapper.GetDirectories(yearFolder);

        var directoriesByDate = directories.Where(d => d.Contains(yearFolder + "/" + date));

        foreach (var folder in directoriesByDate)
        {
            var searchFile = folder + "/" + name + extensionToSearch;

            if (!_ioWrapper.FileExists(searchFile))
            {
                continue;
            }
            
            var finalName = _mediaPathsSettings.MediaPostProcessPath + name + extensionToSearch;
                
            _ioWrapper.CopyFile(searchFile, finalName);
        }*/
    }
}