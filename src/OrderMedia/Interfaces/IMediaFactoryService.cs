// <copyright file="IMediaFactoryService.cs" company="Pablo Bermejo">
// Copyright (c) Pablo Bermejo. All rights reserved.
// </copyright>

namespace OrderMedia.Interfaces
{
    using OrderMedia.MediaFiles;

    /// <summary>
    /// Media factory service interface.
    /// </summary>
    public interface IMediaFactoryService
    {
        /// <summary>
        /// Creates media object based on the path of the file.
        /// </summary>
        /// <param name="path">Full path.</param>
        /// <returns>Media object.</returns>
        BaseMedia CreateMedia(string path);
    }
}
