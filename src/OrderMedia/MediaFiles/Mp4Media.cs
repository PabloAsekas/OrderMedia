// <copyright file="Mp4Media.cs" company="Pablo Bermejo">
// Copyright (c) Pablo Bermejo. All rights reserved.
// </copyright>

namespace OrderMedia.MediaFiles
{
    /// <summary>
    /// Media class for .mov files.
    /// </summary>
    public class Mp4Media : VideoMedia
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Mp4Media"/> class.
        /// </summary>
        /// <param name="path">Media path.</param>
        /// <param name="classificationFolderName">Classification folder name where all the types will be located.</param>
        public Mp4Media(string path, string classificationFolderName)
            : base(path, classificationFolderName)
        {
        }
    }
}
