using System;
using OrderMedia.Interfaces;
using OrderMedia.Models;

namespace OrderMedia.Services.Processors
{
    /// <summary>
    /// Main processor for media files.
    /// </summary>
	public class MainProcessor : IProcessor
	{
        private readonly IIOService _ioService;
        private IProcessor Processor;

        public MainProcessor(IIOService ioService)
        {
            _ioService = ioService;
        }

        public void Execute(Media media)
        {
            _ioService.CreateFolder(media.NewMediaFolder);
            _ioService.MoveMedia(media.MediaPath, media.NewMediaPath);
        }

        public void SetProcessor(IProcessor processor)
        {
            Processor = processor;
        }
    }
}

