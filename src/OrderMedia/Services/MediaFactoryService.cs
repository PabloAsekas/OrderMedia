// <copyright file="MediaFactoryService.cs" company="Pablo Bermejo">
// Copyright (c) Pablo Bermejo. All rights reserved.
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
            string name = Path.GetFileNameWithoutExtension(path);

            extension = this.AnalyzeMediaName(name, "PHOTO", ".WhatsAppImage", extension);
            extension = this.AnalyzeMediaName(name, "VIDEO", ".WhatsAppVideo", extension);

            return extension.ToLower() switch
            {
                ".gif" => new GifMedia(path, this.configurationService.GetImageFolderName()),
                ".heic" => new HeicMedia(path, this.configurationService.GetImageFolderName()),
                ".jpeg" => new JpegMedia(path, this.configurationService.GetImageFolderName()),
                ".jpg" => new JpgMedia(path, this.configurationService.GetImageFolderName()),
                ".png" => new PngMedia(path, this.configurationService.GetImageFolderName()),
                ".mov" => new MovMedia(path, this.configurationService.GetVideoFolderName()),
                ".mp4" => new Mp4Media(path, this.configurationService.GetVideoFolderName()),
                ".whatsappimage" => new WhatsAppMedia(path, this.configurationService.GetImageFolderName()),
                ".whatsappvideo" => new WhatsAppMedia(path, this.configurationService.GetVideoFolderName()),
                ".arw" => new ArwMedia(path, this.configurationService.GetImageFolderName()),
                _ => throw new FormatException($"The provided extension '{extension.ToLower()}' is not supported."),
            };
        }

        /// <summary>
        /// Modify the extension if the media name starts with the given string.
        /// </summary>
        /// <param name="name">Media name.</param>
        /// <param name="startsWith">String that will be compared.</param>
        /// <param name="newExtension">New extension if the name starts with the given string.</param>
        /// <param name="oldExtension">Old extension that will be returned if the name doesn't start with the given string.</param>
        /// <returns>The new extension if the name starts with the given string. The old extension if the name doesn't.</returns>
        private string AnalyzeMediaName(string name, string startsWith, string newExtension, string oldExtension)
        {
            return name.StartsWith(startsWith) ? newExtension : oldExtension;
        }
    }
}
