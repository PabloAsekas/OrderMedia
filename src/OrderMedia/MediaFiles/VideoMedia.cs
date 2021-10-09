// <copyright file="VideoMedia.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace OrderMedia.MediaFiles
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using MetadataExtractor;
    using MetadataExtractor.Formats.QuickTime;

    /// <summary>
    /// Media class for generic video files.
    /// </summary>
    public abstract class VideoMedia : BaseMedia
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VideoMedia"/> class.
        /// </summary>
        /// <param name="path">Media path.</param>
        /// <param name="classificationFolderName">Classification folder name where all the types will be located.</param>
        public VideoMedia(string path, string classificationFolderName)
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

            var quickTimeDirectory = directories.OfType<QuickTimeMetadataHeaderDirectory>().FirstOrDefault();
            var videoCreationDate = quickTimeDirectory?.GetDescription(QuickTimeMetadataHeaderDirectory.TagCreationDate);

            return videoCreationDate;
        }

        private DateTime GetDateTimeFromMetadataString(string metadataString)
        {
            DateTime.TryParseExact(metadataString, "ddd MMM dd HH:mm:ss zzz yyyy", new CultureInfo("en-UK", false), System.Globalization.DateTimeStyles.None, out DateTime videoDate);

            return videoDate;
        }
    }
}
