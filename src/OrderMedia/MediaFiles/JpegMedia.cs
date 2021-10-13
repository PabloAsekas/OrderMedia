// <copyright file="JpegMedia.cs" company="Pablo Bermejo">
// Copyright (c) Pablo Bermejo. All rights reserved.
// </copyright>

namespace OrderMedia.MediaFiles
{
    /// <summary>
    /// Media class for .jpeg files.
    /// </summary>
    public class JpegMedia : ImageMedia
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="JpegMedia"/> class.
        /// </summary>
        /// <param name="path">Media path.</param>
        /// <param name="classificationFolderName">Classification folder name where all the types will be located.</param>
        public JpegMedia(string path, string classificationFolderName)
            : base(path, classificationFolderName)
        {
        }
    }
}
