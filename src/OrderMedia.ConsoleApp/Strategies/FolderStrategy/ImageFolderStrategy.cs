using Microsoft.Extensions.Options;
using OrderMedia.ConsoleApp.Configuration;
using OrderMedia.ConsoleApp.Interfaces;
using OrderMedia.Enums;

namespace OrderMedia.ConsoleApp.Strategies.FolderStrategy;

public class ImageFolderStrategy : IClassificationMediaFolderStrategy
{
    private readonly ClassificationSettings _settings;

    public ImageFolderStrategy(IOptions<ClassificationSettings> settings)
    {
        _settings = settings.Value;
    }

    public bool CanHandle(MediaType mediaType) =>
        mediaType is MediaType.Image 
            or MediaType.Raw
            or MediaType.WhatsAppImage;

    public string GetTargetFolder() =>
        _settings.Folders.ImageFolderName;
}