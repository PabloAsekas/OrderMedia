using System;
using System.Globalization;
using System.Text.RegularExpressions;
using OrderMedia.Interfaces;

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

        public override void PostProcess()
        {
        }

        protected override void SetCreationDate()
        {
            //string pattern = @"[0-9]{4}-(0[1-9]|1[0-2])-(0[1-9]|[1-2][0-9]|3[0-1])";
            //string pattern = @"[0-9]{4}-(0[1-9]|1[0-2])-(0[1-9]|[1-2][0-9]|3[0-1])-(0[0-9]|[10-23]{2})-([0-59]{2})-([0-59]{2})";
            string pattern = @"[0-9]{4}-(0[1-9]|1[0-2])-(0[1-9]|[1-2][0-9]|3[0-1])-(0[0-9]|[1-2][0-9])-([0-5][0-9])-([0-5][0-9])";

            Match m = Regex.Match(Name, pattern, RegexOptions.IgnoreCase);

            // We assume that the regex will succeed.

            SetCreatedDateTimeFromMetadataString(m.Value);
        }

        private void SetCreatedDateTimeFromMetadataString(string metadataString)
        {
            DateTime.TryParseExact(metadataString, "yyyy-MM-dd-HH-mm-ss", new CultureInfo("es-ES", false), System.Globalization.DateTimeStyles.None, out DateTime imageDate);

            CreatedDateTime = imageDate;
        }
    }
}
