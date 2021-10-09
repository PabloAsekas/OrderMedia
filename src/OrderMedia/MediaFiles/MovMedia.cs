// <copyright file="MovMedia.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace OrderMedia.MediaFiles
{
    /// <summary>
    /// Media class for .mov files.
    /// </summary>
    public class MovMedia : VideoMedia
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MovMedia"/> class.
        /// </summary>
        /// <param name="path">Media path.</param>
        /// <param name="classificationFolderName">Classification folder name where all the types will be located.</param>
        public MovMedia(string path, string classificationFolderName)
            : base(path, classificationFolderName)
        {
        }
    }
}
