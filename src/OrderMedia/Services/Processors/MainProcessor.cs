using System;
using OrderMedia.Interfaces;
using OrderMedia.Models;

namespace OrderMedia.Services.Processors
{
    /// <summary>
    /// Main processor for media files.
    /// </summary>
	public class MainProcessor : BaseProcessor
	{
        private readonly IIOService _ioService;

        public MainProcessor(IIOService ioService) : base()
        {
            _ioService = ioService;
        }

        public override void Execute(Media media)
        {
            if (media.CreatedDateTime == default(DateTime))
            {
                return;
            }

            _ioService.CreateFolder(media.NewMediaFolder);
            _ioService.MoveMedia(media.MediaPath, media.NewMediaPath);

            ExecuteProcessors(media);
        }
    }
}

