using System.Collections.Generic;
using OrderMedia.Interfaces;
using OrderMedia.Models;

namespace OrderMedia.Services.Processors
{
    /// <summary>
    /// Processor for Aae files.
    /// </summary>
	public class AaeProcessor : BaseProcessor
	{
        private readonly IIOService _ioService;
        private readonly IRenameService _renameService;

        public AaeProcessor(IIOService ioService, IRenameService renameService) : base()
        {
            _ioService = ioService;
            _renameService = renameService;
        }

        public override void Execute(Media media)
        {
            var possibleNames = new List<string>()
            {
                $"{media.NameWithoutExtension}.aae",
                _renameService.GetAaeName(media.NameWithoutExtension),
            };

            foreach (var aaeName in possibleNames)
            {
                var result = FindAndMove(aaeName, media);

                if (result)
                    break;
            }

            ExecuteProcessors(media);
        }

        private bool FindAndMove(string aaeName, Media media)
        {
            string aaeLocation = _ioService.Combine(new string[] { media.MediaFolder, aaeName });

            if (_ioService.FileExists(aaeLocation))
            {
                string newAaeName = $"{media.NewNameWithoutExtension}.aae";
                string newAaeLocation = _ioService.Combine(new string[] { media.NewMediaFolder, newAaeName });

                _ioService.MoveMedia(aaeLocation, newAaeLocation);

                return true;
            }

            return false;
        }
    }
}
