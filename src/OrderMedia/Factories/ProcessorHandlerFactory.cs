using System;
using OrderMedia.Enums;
using OrderMedia.Handlers.Processor;
using OrderMedia.Interfaces.Factories;
using OrderMedia.Interfaces.Handlers;

namespace OrderMedia.Factories
{
	public class ProcessorHandlerFactory : IProcessorHandlerFactory
	{
        private readonly IServiceProvider _serviceProvider;

        public ProcessorHandlerFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public IProcessorHandler CreateProcessorHandler(MediaType mediaType)
        {
            return mediaType switch
            {
                MediaType.Image => CreateImageProcessorHandler(),
                MediaType.Raw => CreateRawProcessorHandler(),
                MediaType.Video => CreateVideoProcessorHandler(),
                MediaType.WhatsAppImage => CreateWhatsAppImageProcessorHandler(),
                MediaType.WhatsAppVideo => CreateWhatsAppVideoProcessorHandler(),
                MediaType.Insv => CreateVideoProcessorHandler(),
                MediaType.None => throw new FormatException($"The provided media type '{mediaType}' is not supported."),
                _ => throw new FormatException($"The provided media type '{mediaType}' is not supported."),
            };
        }

        private IProcessorHandler CreateImageProcessorHandler()
        {
            var moveLivePhotoProcessorHandler = GetProcessorHandler<MoveLivePhotoProcessorHandler>();
            var moveAaeProcessorHandler = GetProcessorHandler<MoveAaeProcessorHandler>();
            
            var moveMediaProcessorHandler = GetProcessorHandler<MoveMediaProcessorHandler>();

            moveMediaProcessorHandler
                .SetNext(moveLivePhotoProcessorHandler)
                .SetNext(moveAaeProcessorHandler);

            return moveMediaProcessorHandler;
        }

        private IProcessorHandler CreateRawProcessorHandler()
        {
            var moveXmpProcessorHandler = GetProcessorHandler<MoveXmpProcessorHandler>();

            var moveMediaProcessorHandler = GetProcessorHandler<MoveMediaProcessorHandler>();
            
            moveMediaProcessorHandler
                .SetNext(moveXmpProcessorHandler);

            return moveMediaProcessorHandler;
        }

        private IProcessorHandler CreateVideoProcessorHandler()
        {
            return GetProcessorHandler<MoveMediaProcessorHandler>();
        }

        private IProcessorHandler CreateWhatsAppImageProcessorHandler()
        {
            // var createdDateAggregatorProcessorHandler = GetProcessorHandler<CreatedDateAggregatorProcessor>();
            
            var moveMediaProcessorHandler = GetProcessorHandler<MoveMediaProcessorHandler>();
            
            // moveMediaProcessorHandler
                // .SetNext(createdDateAggregatorProcessorHandler);

            return moveMediaProcessorHandler;
        }
        
        private IProcessorHandler CreateWhatsAppVideoProcessorHandler()
        {
            return GetProcessorHandler<MoveMediaProcessorHandler>();
        }

        private IProcessorHandler GetProcessorHandler<T>() where T : IProcessorHandler
        {
            return (IProcessorHandler) _serviceProvider.GetService(typeof(T));
        }
    }
}

