// <copyright file="BaseMedia.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace OrderMedia.MediaFiles
{
    /// <summary>
    /// Base media class. Used to be implemented by the rest of the media classes.
    /// </summary>
    public abstract class BaseMedia
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BaseMedia"/> class.
        /// </summary>
        /// <param name="path">Media path.</param>
        /// <param name="classificationFolderName">Classification folder name where all the types will be located.</param>
        public BaseMedia(string path, string classificationFolderName)
        {
            this.Path = path;
            this.ClassificationFolderName = classificationFolderName;
            this.Name = System.IO.Path.GetFileName(path);
            this.NameWithoutExtension = System.IO.Path.GetFileNameWithoutExtension(path);
        }

        /// <summary>
        /// Gets media path.
        /// </summary>
        public string Path { get; private set; }

        /// <summary>
        /// Gets classification folder name.
        /// </summary>
        public string ClassificationFolderName { get; private set; }

        /// <summary>
        /// Gets name.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Gets name without extension.
        /// </summary>
        public string NameWithoutExtension { get; private set; }

        /// <summary>
        /// Gets creation date.
        /// </summary>
        /// <returns>Creation date string in format yyyy-MM-dd.</returns>
        public abstract string GetCreationDate();
    }
}
