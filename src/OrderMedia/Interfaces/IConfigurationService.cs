// <copyright file="IConfigurationService.cs" company="Pablo Bermejo">
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
    }
}