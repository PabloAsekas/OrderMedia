// <copyright file="HeicMedia.cs" company="Pablo Bermejo">
// Copyright (c) Pablo Bermejo. All rights reserved.
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
    /// Media class for .ARW files.
    /// </summary>
    public class ArwMedia : ImageMedia
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ArwMedia"/> class.
        /// </summary>
        /// <param name="path">Media path.</param>
        /// <param name="classificationFolderName">Classification folder name where all the types will be located.</param>
        public ArwMedia(string path, string classificationFolderName)
            : base(path, classificationFolderName)
        {
        }

        /// <inheritdoc/>
        public override string GetCreationDate()
        {
            var metadataDateTime = this.GetVideoDateFromMetadata(this.Path);

            var finalDateTime = this.GetDateTimeFromMetadataString(metadataDateTime);

            return finalDateTime.ToString("yyyy-MM-dd");
        }

        private string GetVideoDateFromMetadata(string filePath)
        {
            IEnumerable<MetadataExtractor.Directory> directories = ImageMetadataReader.ReadMetadata(filePath);

            var exifDirectory = directories.OfType<ExifIfd0Directory>().FirstOrDefault();
            var imageCreationDate = exifDirectory?.GetDescription(ExifIfd0Directory.TagDateTime);

            return imageCreationDate;
        }

        private DateTime GetDateTimeFromMetadataString(string metadataString)
        {
            DateTime.TryParseExact(metadataString, "yyyy:MM:dd HH:mm:ss", new CultureInfo("es-ES", false), System.Globalization.DateTimeStyles.None, out DateTime imageDate);

            return imageDate;
        }
    }
}