using OrderMedia.Interfaces;

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

        protected override void PostProcess()
        {
            MoveLivePhoto();
            MoveAae();
        }

        private void MoveLivePhoto()
        {
            string videoName = $"{NameWithoutExtension}.mov";
            string videoLocation = _ioService.Combine(new string[] { MediaFolder, videoName });

            string newVideoName = $"{NewNameWithoutExtension}.mov";
            string newVideoLocation = _ioService.Combine(new string[] { NewMediaFolder, newVideoName });

            if (_ioService.Exists(videoLocation))
            {
                _ioService.MoveMedia(videoLocation, newVideoLocation);
            }
        }

        private void MoveAae()
        {
            string aaeName = GetAeeName();
            string aaeLocation = _ioService.Combine(new string[] { MediaFolder, aaeName });

            string newAaeName = $"{NewNameWithoutExtension}.aae";
            string newAaeLocation = _ioService.Combine(new string[] { NewMediaFolder, newAaeName });

            if (_ioService.Exists(aaeLocation))
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
