﻿// <copyright file="IConfigurationService.cs" company="Pablo Bermejo">
// Copyright (c) Pablo Bermejo. All rights reserved.
// </copyright>

namespace OrderMedia.Interfaces
{
    /// <summary>
    /// Configuration service interface.
    /// </summary>
    public interface IConfigurationService
    {
        /// <summary>
        /// Gets image extensions.
        /// </summary>
        /// <returns>String array with the image extensions.</returns>
        string[] GetImageExtensions();

        /// <summary>
        /// Gets video extensions.
        /// </summary>
        /// <returns>String array with the video extensions.</returns>
        string[] GetVideoExtensions();

        /// <summary>
        /// Gets all the media extensions.
        /// </summary>
        /// <returns>String array with the media extensions.</returns>
        string[] GetMediaExtensions();

        /// <summary>
        /// Gets image folder name.
        /// </summary>
        /// <returns>String with the image folder name.</returns>
        string GetImageFolderName();

        /// <summary>
        /// Gets video folder name.
        /// </summary>
        /// <returns>String with the video folder name.</returns>
        string GetVideoFolderName();
    }
}