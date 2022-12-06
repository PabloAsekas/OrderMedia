using System.IO;
using OrderMedia.Interfaces;
using OrderMedia.Services;

namespace OrderMedia.MediaFiles
{
    /// <summary>
    /// Media class for .heic files.
    /// </summary>
    public class HeicMedia : ImageMedia
    {
        public HeicMedia(string mediaPath, string classificationFolderName, IIOService ioService)
            : base(mediaPath, classificationFolderName, ioService)
        {
        }

        public override void PostProcess()
        {
            MoveLivePhoto();
            MoveAae();
        }

        private void MoveLivePhoto()
        {
            string videoName = $"{NameWithoutExtension}.mov";
            string videoLocation = Path.Combine(MediaFolder, videoName);

            if (File.Exists(videoLocation))
            {
                string newVideoLocation = Path.Combine(NewMediaFolder, videoName);
                _ioService.MoveMedia(videoLocation, newVideoLocation);
            }
        }

        private void MoveAae()
        {
            string aaeName = NameWithoutExtension.Insert(4, "O") + ".aae";
            string aaeLocation = Path.Combine(MediaFolder, aaeName);

            if (File.Exists(aaeLocation))
            {
                string newAaeLocation = Path.Combine(NewMediaFolder, aaeName);
                _ioService.MoveMedia(aaeLocation, newAaeLocation);
            }
        }
    }
}
