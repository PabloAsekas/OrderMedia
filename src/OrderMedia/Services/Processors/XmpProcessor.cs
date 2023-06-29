using System;
using OrderMedia.Interfaces;
using OrderMedia.Models;

namespace OrderMedia.Services.Processors
{
    /// <summary>
    /// Processor for Xmp files.
    /// </summary>
    public class XmpProcessor : IProcessor
    {
        private readonly IIOService _ioService;
        private readonly IRenameService _renameService;
        private IProcessor Processor;

        public XmpProcessor(IIOService ioService, IRenameService renameService)
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

            string xmpName = $"{media.NameWithoutExtension}.xmp";
            string xmpLocation = _ioService.Combine(new string[] { media.MediaFolder, xmpName });

            if (_ioService.Exists(xmpLocation))
            {
                string newXmpName = $"{media.NewNameWithoutExtension}.xmp";
                string newXmpLocation = _ioService.Combine(new string[] { media.NewMediaFolder, newXmpName });

                _ioService.MoveMedia(xmpLocation, newXmpLocation);
            }
        }
    }
}

