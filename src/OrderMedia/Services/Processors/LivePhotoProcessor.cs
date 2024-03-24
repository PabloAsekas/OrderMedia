using System.Collections.Generic;
using OrderMedia.Interfaces;
using OrderMedia.Models;

namespace OrderMedia.Services.Processors
{
    /// <summary>
    /// Processor for LivePhoto files.
    /// </summary>
	public class LivePhotoProcessor : BaseProcessor
	{
        private readonly IIOService _ioService;

        public LivePhotoProcessor(IIOService ioService) : base()
        {
            _ioService = ioService;
        }

        public override void Execute(Media media)
        {
            var possibleNames = new List<string>()
            {
                $"{media.NameWithoutExtension}.mov",
                $"{media.NameWithoutExtension}.mp4"
            };

            foreach (var videoName in possibleNames)
            {
                var result = FindAndMove(videoName, media);

                if (result)
                    break;
            }

            ExecuteProcessors(media);
        }

        private bool FindAndMove(string videoName, Media media)
        {
            string videoLocation = _ioService.Combine(new string[] { media.MediaFolder, videoName });

            string extension = _ioService.GetExtension(videoName);

            if (_ioService.FileExists(videoLocation))
            {
                string newVideoName = $"{media.NewNameWithoutExtension}{extension}";
                string newVideoLocation = _ioService.Combine(new string[] { media.NewMediaFolder, newVideoName });

                _ioService.MoveMedia(videoLocation, newVideoLocation);

                return true;
            }

            return false;
        }
    }
}

