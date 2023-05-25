using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using MetadataExtractor;
using MetadataExtractor.Formats.Exif;
using OrderMedia.Interfaces;

namespace OrderMedia.MediaFiles
{
    /// <summary>
    /// Media class for generic images files.
    /// </summary>
    public abstract class ImageMedia : BaseMedia
    {
        public ImageMedia(string mediaPath, string classificationFolderName, IIOService ioService)
            : base(mediaPath, classificationFolderName, ioService)
        {
        }

        public override void PostProcess()
        {
        }

        protected override void SetCreationDate()
        {
            var metadataDateTime = GetDateFromMetadata();

            SetCreatedDateTimeFromMetadataString(metadataDateTime);
        }

        private string GetDateFromMetadata()
        {
            IEnumerable<MetadataExtractor.Directory> directories = ImageMetadataReader.ReadMetadata(MediaPath);

            var exifDirectory = directories.OfType<ExifSubIfdDirectory>().FirstOrDefault();
            var imageCreationDate = exifDirectory?.GetDescription(ExifDirectoryBase.TagDateTimeOriginal);

            return imageCreationDate;
        }

        private void SetCreatedDateTimeFromMetadataString(string metadataString)
        {
            DateTime.TryParseExact(metadataString, "yyyy:MM:dd HH:mm:ss", new CultureInfo("es-ES", false), System.Globalization.DateTimeStyles.None, out DateTime imageDate);

            CreatedDateTime = imageDate;
        }
    }
}
