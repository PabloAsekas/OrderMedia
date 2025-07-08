using System.Linq;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OrderMedia.Interfaces;
using OrderMedia.Configuration;

namespace OrderMedia.Services;

public class CopyComplementFilesService : ICopyComplementFilesService
{
    private readonly IIoWrapper _ioWrapper;
    private readonly ILogger<CopyComplementFilesService> _logger;
    private readonly MediaPathsOptions _mediaPathsOptions;

    public CopyComplementFilesService(
        IIoWrapper ioWrapper,
        ILogger<CopyComplementFilesService> logger,
        IOptions<MediaPathsOptions> mediaPathsOptions)
    {
        _ioWrapper = ioWrapper;
        _logger = logger;
        _mediaPathsOptions = mediaPathsOptions.Value;
    }

    public void CopyComplementFiles(string fileToApply, string extensionToSearch)
    {
        var name = _ioWrapper.GetFileNameWithoutExtension(fileToApply);

        var date = name[..10];

        var year = date[..4];

        var yearFolder = _ioWrapper.Combine([
            _mediaPathsOptions.MediaPostProcessSource,
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
            
            var finalName = _mediaPathsOptions.MediaPostProcessPath + name + extensionToSearch;
                
            _ioWrapper.CopyFile(searchFile, finalName);
        }
    }
}