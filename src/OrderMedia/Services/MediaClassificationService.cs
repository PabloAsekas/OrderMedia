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
        private readonly IMetadataService metadataService;
        private readonly IConfigurationService configurationService;

        /// <summary>
        /// Initializes a new instance of the <see cref="MediaClassificationService"/> class.
        /// </summary>
        /// <param name="logger">Logger.</param>
        /// <param name="ioService">IO Service.</param>
        /// <param name="metadataService">Metadata service.</param>
        /// <param name="configurationService">Configuration service.</param>
        public MediaClassificationService(ILogger<MediaClassificationService> logger, IIOService ioService, IMetadataService metadataService, IConfigurationService configurationService)
        {
            this.logger = logger;
            this.ioService = ioService;
            this.metadataService = metadataService;
            this.configurationService = configurationService;
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

        private void ManageMedia(string type, params string[] extensions) //TODO: refactorizar el type
        {
            var allMedia = this.ioService.GetFilesByExtensions(extensions);

            foreach (var media in allMedia)
            {
                string mediaDate = this.metadataService.GetMediaDate(media.FullName);

                this.ioService.MoveMedia(media, mediaDate, type);
            }
        }

        private void Manage()
        {
            this.ManageMedia("i", this.configurationService.GetImageExtensions()); //TODO: refactorizar el type
            this.ManageMedia("v", this.configurationService.GetVideoExtensions());
        }
    }
}
