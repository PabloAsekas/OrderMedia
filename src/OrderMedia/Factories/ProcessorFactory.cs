using System;
using OrderMedia.Enums;
using OrderMedia.Interfaces;
using OrderMedia.Services.Processors;

namespace OrderMedia.Factories
{
	public class ProcessorFactory : IProcessorFactory
	{
        private readonly IServiceProvider _serviceProvider;

        public ProcessorFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public IProcessor CreateProcessor(MediaType mediaType)
        {
            return mediaType switch
            {
                MediaType.Image => CreateImageProcessor(),
                MediaType.Raw => CreateRawProcessor(),
                MediaType.Video => CreateVideoProcessor(),
                MediaType.WhatsApp => CreateWhatsAppProcessor(),
                _ => throw new FormatException($"The provided media type '{mediaType}' is not supported."),
            };
        }

        private IProcessor CreateImageProcessor()
        {
            var mainProcessor = CreateDefaultProcessor();

            var livePhotoProcessor = (IProcessor) _serviceProvider.GetService(typeof(LivePhotoProcessor));
            livePhotoProcessor.SetProcessor(mainProcessor);

            var aaeProcessor = (IProcessor) _serviceProvider.GetService(typeof(AaeProcessor));
            aaeProcessor.SetProcessor(livePhotoProcessor);

            return aaeProcessor;
        }

        private IProcessor CreateRawProcessor()
        {
            return CreateDefaultProcessor();
        }

        private IProcessor CreateVideoProcessor()
        {
            return CreateDefaultProcessor();
        }

        private IProcessor CreateWhatsAppProcessor()
        {
            return CreateDefaultProcessor();
        }

        private IProcessor CreateDefaultProcessor()
        {
            return (IProcessor) _serviceProvider.GetService(typeof(MainProcessor));
        }
    }
}

