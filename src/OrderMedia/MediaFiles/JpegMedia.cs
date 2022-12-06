using System.IO;
using OrderMedia.Interfaces;
using OrderMedia.Services;

namespace OrderMedia.MediaFiles
{
    /// <summary>
    /// Media class for .jpeg files.
    /// </summary>
    public class JpegMedia : ImageMedia
    {
        public JpegMedia(string mediaPath, string classificationFolderName, IIOService ioService)
            : base(mediaPath, classificationFolderName, ioService)
        {
        }
    }
}
