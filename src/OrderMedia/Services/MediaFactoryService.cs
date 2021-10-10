// <copyright file="MediaFactoryService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace OrderMedia.Services
{
    using System;
    using System.IO;
    using OrderMedia.Interfaces;
    using OrderMedia.MediaFiles;

    /// <summary>
    /// Media factory to create media objects based on the path.
    /// </summary>
    public class MediaFactoryService : IMediaFactoryService
    {
        private readonly IConfigurationService configurationService;

        /// <summary>
        /// Initializes a new instance of the <see cref="MediaFactoryService"/> class.
        /// </summary>
        /// <param name="configurationService">Configuration service.</param>
        public MediaFactoryService(IConfigurationService configurationService)
        {
            this.configurationService = configurationService;
        }

        /// <summary>
        /// Creates media object based on the path of the file.
        /// </summary>
        /// <param name="path">Full path.</param>
        /// <returns>Media object.</returns>
        public BaseMedia CreateMedia(string path)
        {
            string extension = Path.GetExtension(path);

            return extension.ToLower() switch
            {
                ".gif" => new GifMedia(path, this.configurationService.GetImageFolderName()),
                ".heic" => new HeicMedia(path, this.configurationService.GetImageFolderName()),
                ".jpeg" => new JpegMedia(path, this.configurationService.GetImageFolderName()),
                ".jpg" => new JpgMedia(path, this.configurationService.GetImageFolderName()),
                ".png" => new PngMedia(path, this.configurationService.GetImageFolderName()),
                ".mov" => new MovMedia(path, this.configurationService.GetVideoFolderName()),
                ".mp4" => new Mp4Media(path, this.configurationService.GetVideoFolderName()),
                _ => throw new FormatException($"The provided extension '{extension.ToLower()}' is not supported."),
            };
        }
    }
}
