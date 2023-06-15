using OrderMedia.Interfaces;
using OrderMedia.Models;

namespace OrderMedia.Services.Processors
{
    /// <summary>
    /// Processor for Aae files.
    /// </summary>
	public class AaeProcessor : IProcessor
	{
        private readonly IIOService _ioService;
        private readonly IRenameService _renameService;
        private IProcessor Processor;

        public AaeProcessor(IIOService ioService, IRenameService renameService)
        {
            _ioService = ioService;
            _renameService = renameService;
        }

        public void SetProcessor(IProcessor processor)
        {
            Processor = processor;
        }

        public void Execute(Media media)
        {
            Processor?.Execute(media);

            string aaeName = _renameService.GetAaeName(media.NameWithoutExtension);
            string aaeLocation = _ioService.Combine(new string[] { media.MediaFolder, aaeName });

            if (_ioService.Exists(aaeLocation))
            {
                string newAaeName = $"{media.NewNameWithoutExtension}.aae";
                string newAaeLocation = _ioService.Combine(new string[] { media.NewMediaFolder, newAaeName });

                _ioService.MoveMedia(aaeLocation, newAaeLocation);
            }
        }
    }
}

