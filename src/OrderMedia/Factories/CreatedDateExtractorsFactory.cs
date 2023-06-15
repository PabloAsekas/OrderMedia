using System;
using OrderMedia.Enums;
using OrderMedia.Interfaces;
using OrderMedia.Services.CreatedDateExtractors;

namespace OrderMedia.Factories
{
	public class CreatedDateExtractorsFactory : ICreatedDateExtractorsFactory
	{
		private readonly IServiceProvider _serviceProvider;

        public CreatedDateExtractorsFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public ICreatedDateExtractor GetExtractor(MediaType mediaType)
        {
            return mediaType switch
            {
                MediaType.Image => (ICreatedDateExtractor) _serviceProvider.GetService(typeof(ImageCreatedDateExtractor)),
                MediaType.Raw => (ICreatedDateExtractor) _serviceProvider.GetService(typeof(RawCreatedDateExtractor)),
                MediaType.Video => (ICreatedDateExtractor) _serviceProvider.GetService(typeof(VideoCreatedDateExtractor)),
                MediaType.WhatsApp => (ICreatedDateExtractor) _serviceProvider.GetService(typeof(WhatsAppCreatedDateExtractor)),
                _ => throw new FormatException($"The provided media type '{mediaType}' is not supported."),
            };
        }
    }
}

