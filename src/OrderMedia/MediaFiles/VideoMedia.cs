using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using MetadataExtractor;
using MetadataExtractor.Formats.QuickTime;
using OrderMedia.Interfaces;
using OrderMedia.Services;

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

        public override string GetCreationDate()
        {
            var metadataDateTime = GetVideoDateFromMetadata();

            SetCreatedDateTimeFromMetadataString(metadataDateTime);

            return CreatedDateTime.ToString("yyyy-MM-dd");
        }

        public override void PostProcess()
        {
        }

        private string GetVideoDateFromMetadata()
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
