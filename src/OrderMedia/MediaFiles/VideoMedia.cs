using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using MetadataExtractor;
using MetadataExtractor.Formats.QuickTime;
using OrderMedia.Interfaces;

namespace OrderMedia.MediaFiles
{
    /// <summary>
    /// Media class for generic video files.
    /// </summary>
    public abstract class VideoMedia : BaseMedia
    {
        public VideoMedia(string mediaPath, string classificationFolderName, IIOService ioService)
            : base(mediaPath, classificationFolderName, ioService)
        {
        }

        protected override void PostProcess()
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

            var quickTimeDirectory = directories.OfType<QuickTimeMetadataHeaderDirectory>().FirstOrDefault();
            var videoCreationDate = quickTimeDirectory?.GetDescription(QuickTimeMetadataHeaderDirectory.TagCreationDate);

            return videoCreationDate;
        }

        private void SetCreatedDateTimeFromMetadataString(string metadataString)
        {
            DateTime.TryParseExact(metadataString, "ddd MMM dd HH:mm:ss zzz yyyy", new CultureInfo("en-UK", false), System.Globalization.DateTimeStyles.None, out DateTime videoDate);

            CreatedDateTime = videoDate;
        }
    }
}
