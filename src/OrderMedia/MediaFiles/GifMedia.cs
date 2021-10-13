// <copyright file="GifMedia.cs" company="Pablo Bermejo">
// Copyright (c) Pablo Bermejo. All rights reserved.
// </copyright>

namespace OrderMedia.MediaFiles
{
    /// <summary>
    /// Media class for .gif files.
    /// </summary>
    public class GifMedia : ImageMedia
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GifMedia"/> class.
        /// </summary>
        /// <param name="path">Media gif.</param>
        /// <param name="classificationFolderName">Classification folder name where all the types will be located.</param>
        public GifMedia(string path, string classificationFolderName)
            : base(path, classificationFolderName)
        {
        }
    }
}
