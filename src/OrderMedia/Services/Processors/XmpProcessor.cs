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

        public XmpProcessor(IIOService ioService) : base()
        {
            _ioService = ioService;
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

