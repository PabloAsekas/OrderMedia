// <copyright file="IMetadataService.cs" company="Pablo Bermejo">
// Copyright (c) Pablo Bermejo. All rights reserved.
// </copyright>

namespace OrderMedia.Interfaces
{
    /// <summary>
    /// IMetadataService interface.
    /// </summary>
    public interface IMetadataService
    {
        /// <summary>
        /// Gets image date.
        /// </summary>
        /// <param name="filePath">File path.</param>
        /// <returns>String with the date in format yyyy-mm-dd.</returns>
        string GetMediaDate(string filePath);
    }
}
