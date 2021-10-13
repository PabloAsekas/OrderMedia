// <copyright file="HeicMedia.cs" company="Pablo Bermejo">
// Copyright (c) Pablo Bermejo. All rights reserved.
// </copyright>

namespace OrderMedia.MediaFiles
{
    /// <summary>
    /// Media class for .heic files.
    /// </summary>
    public class HeicMedia : ImageMedia
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HeicMedia"/> class.
        /// </summary>
        /// <param name="path">Media path.</param>
        /// <param name="classificationFolderName">Classification folder name where all the types will be located.</param>
        public HeicMedia(string path, string classificationFolderName)
            : base(path, classificationFolderName)
        {
        }
    }
}
