using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using MetadataExtractor;
using MetadataExtractor.Formats.Exif;
using OrderMedia.Interfaces;
using OrderMedia.Services;

namespace OrderMedia.MediaFiles
{
    /// <summary>
    /// Media class for WhatsApp media files.
    /// </summary>
    public class WhatsAppMedia : BaseMedia
    {
        public WhatsAppMedia(string mediaPath, string classificationFolderName, IIOService ioService)
            : base(mediaPath, classificationFolderName, ioService)
        {
        }

        public override string GetCreationDate()
        {
            //string pattern = @"[0-9]{4}-(0[1-9]|1[0-2])-(0[1-9]|[1-2][0-9]|3[0-1])";
            //string pattern = @"[0-9]{4}-(0[1-9]|1[0-2])-(0[1-9]|[1-2][0-9]|3[0-1])-(0[0-9]|[10-23]{2})-([0-59]{2})-([0-59]{2})";
            string pattern = @"[0-9]{4}-(0[1-9]|1[0-2])-(0[1-9]|[1-2][0-9]|3[0-1])-(0[0-9]|[1-2][0-9])-([0-5][0-9])-([0-5][0-9])";

            Match m = Regex.Match(Name, pattern, RegexOptions.IgnoreCase);

            // We assume that the regex will succeed.

            SetCreatedDateTimeFromMetadataString(m.Value);

            return CreatedDateTime.ToString("yyyy-MM-dd");
        }

        public override void PostProcess()
        {
            // no funciona
            /*if (File.Exists(NewMediaPath))
            {
                File.SetCreationTime(NewMediaPath, new DateTime(2022, 11, 02, 12, 26, 08));
                File.SetLastWriteTime(NewMediaPath, new DateTime(2022, 11, 02, 12, 26, 08));
            }*/
        }

        private void SetCreatedDateTimeFromMetadataString(string metadataString)
        {
            DateTime.TryParseExact(metadataString, "yyyy-MM-dd-HH-mm-ss", new CultureInfo("es-ES", false), System.Globalization.DateTimeStyles.None, out DateTime imageDate);

            CreatedDateTime = imageDate;
        }
    }
}
