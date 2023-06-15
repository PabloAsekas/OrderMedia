using OrderMedia.Interfaces;
using OrderMedia.Models;

namespace OrderMedia.Services.Processors
{
    /// <summary>
    /// Processor for LivePhoto files.
    /// </summary>
	public class LivePhotoProcessor : IProcessor
	{
        private readonly IIOService _ioService;
        private IProcessor Processor;

        public LivePhotoProcessor(IIOService ioService)
        {
            _ioService = ioService;
        }

        public void SetProcessor(IProcessor processor)
        {
            Processor = processor;
        }

        public void Execute(Media media)
        {
            Processor?.Execute(media);

            string videoName = $"{media.NameWithoutExtension}.mov";
            string videoLocation = _ioService.Combine(new string[] { media.MediaFolder, videoName });

            if (_ioService.Exists(videoLocation))
            {
                string newVideoName = $"{media.NewNameWithoutExtension}.mov";
                string newVideoLocation = _ioService.Combine(new string[] { media.NewMediaFolder, newVideoName });

                _ioService.MoveMedia(videoLocation, newVideoLocation);
            }
        }
    }
}

