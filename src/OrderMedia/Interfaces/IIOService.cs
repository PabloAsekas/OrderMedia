// <copyright file="IIOService.cs" company="Pablo Bermejo">
// Copyright (c) Pablo Bermejo. All rights reserved.
// </copyright>

namespace OrderMedia.Interfaces
{
    using System.Collections.Generic;
    using System.IO;
    using OrderMedia.MediaFiles;

    /// <summary>
    /// IIOService interface.
    /// </summary>
    public interface IIOService
    {
        /// <summary>
        /// Creates media folders needed for the classification.
        /// </summary>
        void CreateMediaFolders();

        /// <summary>
        /// Gets files by the given extensions.
        /// </summary>
        /// <param name="extensions">Extensions array.</param>
        /// <returns><see cref="IEnumerable{T}"/> of files that match the given extensions.</returns>
        IEnumerable<FileInfo> GetFilesByExtensions(params string[] extensions);

        /// <summary>
        /// Moves media to the desire sub folder.
        /// </summary>
        /// <param name="file">File to be moved.</param>
        /// <param name="subFolderName">Name of the sub folder.</param>
        void MoveMedia(BaseMedia file, string subFolderName);
    }
}