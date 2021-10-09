// <copyright file="ImageMedia.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace OrderMedia.MediaFiles
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using MetadataExtractor;
    using MetadataExtractor.Formats.Exif;

    /// <summary>
    /// Media class for generic images files.
    /// </summary>
    public abstract class ImageMedia : BaseMedia
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ImageMedia"/> class.
        /// </summary>
        /// <param name="path">Media path.</param>
        /// <param name="classificationFolderName">Classification folder name where all the types will be located.</param>
        public ImageMedia(string path, string classificationFolderName)
            : base(path, classificationFolderName)
        {
        }

        /// <inheritdoc/>
        public override string GetCreationDate()
        {
            var metadataDateTime = this.GetImageDateFromMetadata(this.Path);

            var finalDateTime = this.GetDateTimeFromMetadataString(metadataDateTime);

            return finalDateTime.ToString("yyyy-MM-dd");
        }

        private string GetImageDateFromMetadata(string filePath)
        {
            IEnumerable<MetadataExtractor.Directory> directories = ImageMetadataReader.ReadMetadata(filePath);

            var exifDirectory = directories.OfType<ExifSubIfdDirectory>().FirstOrDefault();
            var imageCreationDate = exifDirectory?.GetDescription(ExifDirectoryBase.TagDateTimeOriginal);

            return imageCreationDate;
        }

        private DateTime GetDateTimeFromMetadataString(string metadataString)
        {
            DateTime.TryParseExact(metadataString, "yyyy:MM:dd HH:mm:ss", new CultureInfo("es-ES", false), System.Globalization.DateTimeStyles.None, out DateTime imageDate);

            return imageDate;
        }
    }
}
