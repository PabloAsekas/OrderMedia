using OrderMedia.Interfaces;
using OrderMedia.Services;

namespace OrderMedia.MediaFiles
{
    /// <summary>
    /// Media class for .jpg files.
    /// </summary>
    public class JpgMedia : ImageMedia
    {
        public JpgMedia(string mediaPath, string classificationFolderName, IIOService ioService)
            : base(mediaPath, classificationFolderName, ioService)
        {
        }
    }
}
