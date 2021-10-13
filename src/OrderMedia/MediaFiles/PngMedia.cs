// <copyright file="PngMedia.cs" company="Pablo Bermejo">
// Copyright (c) Pablo Bermejo. All rights reserved.
// </copyright>

namespace OrderMedia.MediaFiles
{
    /// <summary>
    /// Media class for .png files.
    /// </summary>
    public class PngMedia : ImageMedia
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PngMedia"/> class.
        /// </summary>
        /// <param name="path">Media path.</param>
        /// <param name="classificationFolderName">Classification folder name where all the types will be located.</param>
        public PngMedia(string path, string classificationFolderName)
            : base(path, classificationFolderName)
        {
        }
    }
}
