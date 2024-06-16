using Microsoft.Extensions.Logging;
using OrderMedia.Interfaces;
using OrderMedia.Interfaces.Factories;

namespace OrderMedia.ConsoleApp.Services
{
    /// <summary>
    /// Order Media service class.
    /// </summary>
    public class OrderMediaService
    {
        private readonly ILogger<OrderMediaService> _logger;
        private readonly IIOService _ioService;
        private readonly IConfigurationService _configurationService;
        private readonly IMediaFactory _mediaFactory;
        private readonly IClassificationService _classificationService;

        public OrderMediaService(ILogger<OrderMediaService> logger, IIOService ioService, IConfigurationService configurationService, IMediaFactory mediaFactoryService, IClassificationService classificationService)
        {
            _logger = logger;
            _ioService = ioService;
            _configurationService = configurationService;
            _mediaFactory = mediaFactoryService;
            _classificationService = classificationService;
        }

        public void Run()
        {
            _logger.LogInformation("Classification started.");

            CreateMediaFolders();

            Manage();

            _logger.LogInformation("Classification ended.");
        }

        private void CreateMediaFolders()
        {
            _ioService.CreateFolder(_ioService.Combine(new string[] { _configurationService.GetMediaSourcePath(), _configurationService.GetImageFolderName() }));
            _ioService.CreateFolder(_ioService.Combine(new string[] { _configurationService.GetMediaSourcePath(), _configurationService.GetVideoFolderName() }));
        }

        private void ManageMedia(params string[] extensions)
        {
            var allMedia = _ioService.GetFilesByExtensions(_configurationService.GetMediaSourcePath(), extensions);

            foreach (var media in allMedia)
            {
                var mediaObject = _mediaFactory.CreateMedia(media.FullName);

                _classificationService.Process(mediaObject);
            }
        }

        private void Manage()
        {
            // Images first because of livePhotos classification.
            ManageMedia(_configurationService.GetImageExtensions());
            ManageMedia(_configurationService.GetVideoExtensions());
        }
    }
}
