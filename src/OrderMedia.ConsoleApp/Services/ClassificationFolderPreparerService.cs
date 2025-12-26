using Microsoft.Extensions.Options;
using OrderMedia.ConsoleApp.Configuration;
using OrderMedia.ConsoleApp.Interfaces;
using OrderMedia.Interfaces;

namespace OrderMedia.ConsoleApp.Services;

public class ClassificationFolderPreparerService : IClassificationFolderPreparer
{
    private readonly ClassificationSettings _settings;
    private readonly IEnumerable<IClassificationMediaFolderStrategy> _strategies;
    private readonly IIoWrapper _ioWrapper;
    
    public ClassificationFolderPreparerService(
        IOptions<ClassificationSettings> classificationSettingsOptions,
        IEnumerable<IClassificationMediaFolderStrategy> strategies,
        IIoWrapper ioWrapper)
    {
        _settings = classificationSettingsOptions.Value;
        _strategies = strategies;
        _ioWrapper = ioWrapper;
    }
    
    public void Prepare()
    {
        foreach (var strategy in _strategies)
        {
            var folder = _ioWrapper.Combine([_settings.MediaSourcePath, strategy.GetTargetFolder()]);
            
            _ioWrapper.CreateFolder(folder);
        }
    }
}