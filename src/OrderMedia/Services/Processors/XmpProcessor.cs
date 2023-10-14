using OrderMedia.Interfaces;
using OrderMedia.Models;

namespace OrderMedia.Services.Processors
{
    /// <summary>
    /// Processor for Xmp files.
    /// </summary>
    public class XmpProcessor : BaseProcessor
    {
        private readonly IIOService _ioService;
        private readonly IRenameService _renameService;

        public XmpProcessor(IIOService ioService, IRenameService renameService) : base()
        {
            _ioService = ioService;
            _renameService = renameService;
        }

        public override void Execute(Media media)
        {
            string xmpName = $"{media.NameWithoutExtension}.xmp";
            string xmpLocation = _ioService.Combine(new string[] { media.MediaFolder, xmpName });

            if (_ioService.Exists(xmpLocation))
            {
                string newXmpName = $"{media.NewNameWithoutExtension}.xmp";
                string newXmpLocation = _ioService.Combine(new string[] { media.NewMediaFolder, newXmpName });

                _ioService.MoveMedia(xmpLocation, newXmpLocation);
            }

            ExecuteProcessors(media);
        }
    }
}

