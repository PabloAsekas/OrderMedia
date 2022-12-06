using OrderMedia.Interfaces;
using OrderMedia.Services;

namespace OrderMedia.MediaFiles
{
    /// <summary>
    /// Media class for .gif files.
    /// </summary>
    public class GifMedia : ImageMedia
    {
        public GifMedia(string mediaPath, string classificationFolderName, IIOService ioService)
            : base(mediaPath, classificationFolderName, ioService)
        {
        }
    }
}
