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
                MediaType.WhatsAppImage => CreateWhatsAppImageProcessor(),
                MediaType.WhatsAppVideo => CreateWhatsAppVideoProcessor(),
                _ => throw new FormatException($"The provided media type '{mediaType}' is not supported."),
            };
        }

        private IProcessor CreateImageProcessor()
        {
            var livePhotoProcessor = (IProcessor) _serviceProvider.GetService(typeof(LivePhotoProcessor));
            var aaeProcessor = (IProcessor) _serviceProvider.GetService(typeof(AaeProcessor));
            
            var mainProcessor = CreateDefaultProcessor();

            mainProcessor.AddProcessor(livePhotoProcessor);
            mainProcessor.AddProcessor(aaeProcessor);

            return mainProcessor;
        }

        private IProcessor CreateRawProcessor()
        {
            var xmpProcessor = (IProcessor)_serviceProvider.GetService(typeof(XmpProcessor));

            var mainProcessor = CreateDefaultProcessor();
            mainProcessor.AddProcessor(xmpProcessor);

            return mainProcessor;
        }

        private IProcessor CreateVideoProcessor()
        {
            return CreateDefaultProcessor();
        }

        private IProcessor CreateWhatsAppImageProcessor()
        {
            var createdDateProcessor = (IProcessor)_serviceProvider.GetService(typeof(CreatedDateProcessor));
            
            var mainProcessor = CreateDefaultProcessor();
            mainProcessor.AddProcessor(createdDateProcessor);

            return mainProcessor;
        }
        
        private IProcessor CreateWhatsAppVideoProcessor()
        {
            return CreateDefaultProcessor();
        }

        private BaseProcessor CreateDefaultProcessor()
        {
            return (BaseProcessor) _serviceProvider.GetService(typeof(MainProcessor));
        }
    }
}

