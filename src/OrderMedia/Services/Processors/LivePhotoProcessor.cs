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
            string videoName = $"{media.NameWithoutExtension}.mov";
            string videoLocation = _ioService.Combine(new string[] { media.MediaFolder, videoName });

            if (_ioService.Exists(videoLocation))
            {
                string newVideoName = $"{media.NewNameWithoutExtension}.mov";
                string newVideoLocation = _ioService.Combine(new string[] { media.NewMediaFolder, newVideoName });

                _ioService.MoveMedia(videoLocation, newVideoLocation);
            }

            ExecuteProcessors(media);
        }
    }
}

