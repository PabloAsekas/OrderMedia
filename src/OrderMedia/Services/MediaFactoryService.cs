using System;
using System.IO;
using OrderMedia.Interfaces;
using OrderMedia.MediaFiles;

namespace OrderMedia.Services
{
    /// <summary>
    /// Media factory to create media objects based on the path.
    /// </summary>
    public class MediaFactoryService : IMediaFactoryService
    {
        private readonly IConfigurationService _configurationService;
        private readonly IIOService _ioService;

        /// <summary>
        /// Initializes a new instance of the <see cref="MediaFactoryService"/> class.
        /// </summary>
        /// <param name="configurationService">Configuration service.</param>
        public MediaFactoryService(IConfigurationService configurationService, IIOService ioService)
        {
            _configurationService = configurationService;
            _ioService = ioService;
        }

        public BaseMedia CreateMedia(string path)
        {
            string extension = Path.GetExtension(path);
            string name = Path.GetFileNameWithoutExtension(path);

            extension = AnalyzeMediaName(name, "PHOTO", ".WhatsAppImage", extension);
            extension = AnalyzeMediaName(name, "VIDEO", ".WhatsAppVideo", extension);

            return extension.ToLower() switch
            {
                ".gif" => new GifMedia(path, _configurationService.GetImageFolderName(), _ioService),
                ".heic" => new HeicMedia(path, _configurationService.GetImageFolderName(), _ioService),
                ".jpeg" => new JpegMedia(path, _configurationService.GetImageFolderName(), _ioService),
                ".jpg" => new JpgMedia(path, _configurationService.GetImageFolderName(), _ioService),
                ".png" => new PngMedia(path, _configurationService.GetImageFolderName(), _ioService),
                ".mov" => new MovMedia(path, _configurationService.GetVideoFolderName(), _ioService),
                ".mp4" => new Mp4Media(path, _configurationService.GetVideoFolderName(), _ioService),
                ".whatsappimage" => new WhatsAppMedia(path, _configurationService.GetImageFolderName(), _ioService),
                ".whatsappvideo" => new WhatsAppMedia(path, _configurationService.GetVideoFolderName(), _ioService),
                ".arw" => new ArwMedia(path, _configurationService.GetImageFolderName(), _ioService),
                ".dng" => new DngMedia(path, _configurationService.GetImageFolderName(), _ioService),
                _ => throw new FormatException($"The provided extension '{extension.ToLower()}' is not supported."),
            };
        }

        private static string AnalyzeMediaName(string name, string startsWith, string newExtension, string oldExtension)
        {
            return name.StartsWith(startsWith) ? newExtension : oldExtension;
        }
    }
}
