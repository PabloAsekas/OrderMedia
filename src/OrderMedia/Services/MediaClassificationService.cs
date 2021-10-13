// <copyright file="MediaClassificationService.cs" company="Pablo Bermejo">
// Copyright (c) Pablo Bermejo. All rights reserved.
// </copyright>

namespace OrderMedia.Services
{
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;
    using OrderMedia.Interfaces;

    /// <summary>
    /// Media classification service class.
    /// </summary>
    public class MediaClassificationService : IHostedService
    {
        private readonly ILogger<MediaClassificationService> logger;
        private readonly IIOService ioService;
        private readonly IConfigurationService configurationService;
        private readonly IMediaFactoryService mediaFactoryService;

        /// <summary>
        /// Initializes a new instance of the <see cref="MediaClassificationService"/> class.
        /// </summary>
        /// <param name="logger">Logger.</param>
        /// <param name="ioService">IO Service.</param>
        /// <param name="configurationService">Configuration service.</param>
        /// <param name="mediaFactoryService">Media factory service.</param>
        public MediaClassificationService(ILogger<MediaClassificationService> logger, IIOService ioService, IConfigurationService configurationService, IMediaFactoryService mediaFactoryService)
        {
            this.logger = logger;
            this.ioService = ioService;
            this.configurationService = configurationService;
            this.mediaFactoryService = mediaFactoryService;
        }

        /// <inheritdoc/>
        public Task StartAsync(CancellationToken cancellationToken)
        {
            this.logger.LogInformation("Classification started.");

            this.ioService.CreateMediaFolders();

            this.Manage();

            return Task.CompletedTask;
        }

        /// <inheritdoc/>
        public Task StopAsync(CancellationToken cancellationToken)
        {
            this.logger.LogInformation("Classification ended.");

            return Task.CompletedTask;
        }

        private void ManageMedia(params string[] extensions)
        {
            var allMedia = this.ioService.GetFilesByExtensions(extensions);

            foreach (var media in allMedia)
            {
                var mediaObject = this.mediaFactoryService.CreateMedia(media.FullName);

                string mediaDate = mediaObject.GetCreationDate();

                this.ioService.MoveMedia(mediaObject, mediaDate);
            }
        }

        private void Manage()
        {
            // Images first because of livePhotos classification.
            this.ManageMedia(this.configurationService.GetImageExtensions());
            this.ManageMedia(this.configurationService.GetVideoExtensions());
        }
    }
}
