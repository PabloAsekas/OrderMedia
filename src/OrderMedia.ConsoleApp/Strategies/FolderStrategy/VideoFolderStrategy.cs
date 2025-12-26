using Microsoft.Extensions.Options;
using OrderMedia.ConsoleApp.Configuration;
using OrderMedia.ConsoleApp.Interfaces;
using OrderMedia.Enums;

namespace OrderMedia.ConsoleApp.Strategies.FolderStrategy;

public class VideoFolderStrategy : IClassificationMediaFolderStrategy
{
    private readonly ClassificationSettings _settings;

    public VideoFolderStrategy(IOptions<ClassificationSettings> settings)
    {
        _settings = settings.Value;
    }

    public bool CanHandle(MediaType mediaType) =>
        mediaType is MediaType.Video 
            or MediaType.WhatsAppVideo 
            or MediaType.Insv;

    public string GetTargetFolder() =>
        _settings.Folders.VideoFolderName;
}