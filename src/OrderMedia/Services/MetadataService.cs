// <copyright file="MetadataService.cs" company="Pablo Bermejo">
// Copyright (c) Pablo Bermejo. All rights reserved.
// </copyright>

namespace OrderMedia.Services
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using MetadataExtractor;
    using MetadataExtractor.Formats.Exif;
    using MetadataExtractor.Formats.QuickTime;
    using OrderMedia.Interfaces;

    /// <summary>
    /// Metadata service class.
    /// </summary>
    public class MetadataService : IMetadataService
    {
        /// <summary>
        /// Gets image date.
        /// </summary>
        /// <param name="filePath">File path.</param>
        /// <returns>String with the date in format yyyy-mm-dd.</returns>
        /*public string GetMediaDate(string filePath)
        {
            var metadataDateTime = this.GetImageDateFromMetadata(filePath);

            var finalDateTime = this.GetDateTimeFromMetadataString(metadataDateTime);

            return finalDateTime.ToString("yyyy-MM-dd");
        }

        private string GetImageDateFromMetadata(string filePath)
        {
            IEnumerable<MetadataExtractor.Directory> directories = ImageMetadataReader.ReadMetadata(filePath);

            var exifDirectory = directories.OfType<ExifSubIfdDirectory>().FirstOrDefault();
            var imageCreationDate = exifDirectory?.GetDescription(ExifDirectoryBase.TagDateTimeOriginal);

            //var quickTimeDirectory = directories.OfType<QuickTimeMovieHeaderDirectory>().FirstOrDefault();
            var quickTimeDirectory = directories.OfType<QuickTimeMetadataHeaderDirectory>().FirstOrDefault();
            //var videoCreationDate = quickTimeDirectory?.GetDescription(QuickTimeMovieHeaderDirectory.TagCreated);
            var videoCreationDate = quickTimeDirectory?.GetDescription(QuickTimeMetadataHeaderDirectory.TagCreationDate);

            return imageCreationDate ?? videoCreationDate;
        }

        private DateTime GetDateTimeFromMetadataString(string metadataString)
        {
            var imageDateParsed = DateTime.TryParseExact(metadataString, "yyyy:MM:dd HH:mm:ss", new CultureInfo("es-ES", false), System.Globalization.DateTimeStyles.None, out DateTime imageDate);

            DateTime.TryParseExact(metadataString, "ddd MMM dd HH:mm:ss zzz yyyy", new CultureInfo("en-UK", false), System.Globalization.DateTimeStyles.None, out DateTime videoDate);

            DateTime result = imageDateParsed ? imageDate : videoDate;

            return result;
        }*/
    }
}
