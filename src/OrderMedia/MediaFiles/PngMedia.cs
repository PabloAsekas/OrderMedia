using OrderMedia.Interfaces;

namespace OrderMedia.MediaFiles
{
    /// <summary>
    /// Media class for .png files.
    /// </summary>
    public class PngMedia : ImageMedia
    {
        public PngMedia(string mediaPath, string classificationFolderName, IIOService ioService)
            : base(mediaPath, classificationFolderName, ioService)
        {
        }
    }
}
