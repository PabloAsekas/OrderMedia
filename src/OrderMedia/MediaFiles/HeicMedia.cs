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

            string newVideoName = $"{NewNameWithoutExtension}.mov";
            string newVideoLocation = Path.Combine(NewMediaFolder, newVideoName);

            if (File.Exists(videoLocation))
            {
                _ioService.MoveMedia(videoLocation, newVideoLocation);
            }
        }

        private void MoveAae()
        {
            string aaeName = GetAeeName();
            string aaeLocation = Path.Combine(MediaFolder, aaeName);

            string newAaeName = $"{NewNameWithoutExtension}.aae";
            string newAaeLocation = Path.Combine(NewMediaFolder, newAaeName);

            if (File.Exists(aaeLocation))
            {
                _ioService.MoveMedia(aaeLocation, newAaeLocation);
            }
        }

        private string GetAeeName()
        {
            // Images with the (1) have the aae as IMG_xxxx (1)O.aae 
            if (NameWithoutExtension.Contains('('))
            {
                return $"{NameWithoutExtension}O.aae";
            }

            // Images with regular names have the aae as IMG_Oxxx.aae
            return $"{NameWithoutExtension.Insert(4, "O")}.aae";
        }
    }
}
