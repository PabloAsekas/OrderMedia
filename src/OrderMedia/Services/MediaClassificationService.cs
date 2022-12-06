using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using OrderMedia.Interfaces;

namespace OrderMedia.Services
{
    /// <summary>
    /// Media classification service class.
    /// </summary>
    public class MediaClassificationService : IHostedService
    {
        private readonly ILogger<MediaClassificationService> _logger;
        private readonly IIOService _ioService;
        private readonly IConfigurationService _configurationService;
        private readonly IMediaFactoryService _mediaFactoryService;

        /// <summary>
        /// Initializes a new instance of the <see cref="MediaClassificationService"/> class.
        /// </summary>
        /// <param name="logger">Logger.</param>
        /// <param name="ioService">IO Service.</param>
        /// <param name="configurationService">Configuration service.</param>
        /// <param name="mediaFactoryService">Media factory service.</param>
        public MediaClassificationService(ILogger<MediaClassificationService> logger, IIOService ioService, IConfigurationService configurationService, IMediaFactoryService mediaFactoryService)
        {
            _logger = logger;
            _ioService = ioService;
            _configurationService = configurationService;
            _mediaFactoryService = mediaFactoryService;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Classification started.");

            CreateMediaFolders();

            Manage();

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Classification ended.");

            return Task.CompletedTask;
        }

        private void CreateMediaFolders()
        {
            _ioService.CreateFolder(Path.Combine(_configurationService.GetOriginalPath(), _configurationService.GetImageFolderName()));
            _ioService.CreateFolder(Path.Combine(_configurationService.GetOriginalPath(), _configurationService.GetVideoFolderName()));
        }

        private void ManageMedia(params string[] extensions)
        {
            var allMedia = _ioService.GetFilesByExtensions(_configurationService.GetOriginalPath(), extensions);

            foreach (var media in allMedia)
            {
                var mediaObject = _mediaFactoryService.CreateMedia(media.FullName);

                mediaObject.Process();

                //string mediaDate = mediaObject.GetCreationDate();

                //this.ioService.MoveMedia(mediaObject, mediaDate);
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
