// <copyright file="JpgMedia.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace OrderMedia.MediaFiles
{
    /// <summary>
    /// Media class for .jpg files.
    /// </summary>
    public class JpgMedia : HeicMedia
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="JpgMedia"/> class.
        /// </summary>
        /// <param name="path">Media path.</param>
        /// <param name="classificationFolderName">Classification folder name where all the types will be located.</param>
        public JpgMedia(string path, string classificationFolderName)
            : base(path, classificationFolderName)
        {
        }
    }
}
