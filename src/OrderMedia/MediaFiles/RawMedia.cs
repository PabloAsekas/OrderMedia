using System;
using MetadataExtractor;
using MetadataExtractor.Formats.Exif;
using System.Collections.Generic;
using System.Globalization;
using OrderMedia.Interfaces;
using System.Linq;

namespace OrderMedia.MediaFiles
{
	public abstract class RawMedia : ImageMedia
	{
        public RawMedia(string mediaPath, string classificationFolderName, IIOService ioService)
            : base(mediaPath, classificationFolderName, ioService)
        {
        }

        public override string GetCreationDate()
        {
            var metadataDateTime = GetVideoDateFromMetadata();

            SetCreatedDateTimeFromMetadataString(metadataDateTime);

            return CreatedDateTime.ToString("yyyy-MM-dd");
        }

        private string GetVideoDateFromMetadata()
        {
            IEnumerable<MetadataExtractor.Directory> directories = ImageMetadataReader.ReadMetadata(MediaPath);

            var exifDirectory = directories.OfType<ExifIfd0Directory>().FirstOrDefault();
            var imageCreationDate = exifDirectory?.GetDescription(ExifIfd0Directory.TagDateTime);

            return imageCreationDate;
        }

        private void SetCreatedDateTimeFromMetadataString(string metadataString)
        {
            DateTime.TryParseExact(metadataString, "yyyy:MM:dd HH:mm:ss", new CultureInfo("es-ES", false), System.Globalization.DateTimeStyles.None, out DateTime imageDate);

            CreatedDateTime = imageDate;
        }
    }
}

