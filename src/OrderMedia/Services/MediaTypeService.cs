using System;
using OrderMedia.Enums;
using OrderMedia.Interfaces;

namespace OrderMedia.Services
{
    /// <summary>
    /// Media Type Service.
    /// </summary>
	public class MediaTypeService : IMediaTypeService
	{
        private readonly IIOService _ioService;

        public MediaTypeService(IIOService ioService)
        {
            _ioService = ioService;
        }

        public MediaType GetMediaType(string path)
        {
            var extension = _ioService.GetExtension(path);
            var name = _ioService.GetFileNameWithoutExtension(path);

            extension = AnalyzeMediaName(name, "PHOTO", ".WhatsAppImage", extension);
            extension = AnalyzeMediaName(name, "VIDEO", ".WhatsAppVideo", extension);
            extension = AnalyzeMediaName(name, "GIF", ".WhatsAppImage", extension);

            return extension.ToLower() switch
            {
                ".gif" => MediaType.Image,
                ".heic" => MediaType.Image,
                ".jpeg" => MediaType.Image,
                ".jpg" => MediaType.Image,
                ".png" => MediaType.Image,
                ".mov" => MediaType.Video,
                ".mp4" => MediaType.Video,
                ".whatsappimage" => MediaType.WhatsAppImage,
                ".whatsappvideo" => MediaType.WhatsAppVideo,
                ".arw" => MediaType.Raw,
                ".dng" => MediaType.Raw,
                ".insp" => MediaType.Image,
                ".insv" => MediaType.Insv,
                ".cr2" => MediaType.Image,
                ".cr3" => MediaType.Image,
                _ => throw new FormatException($"The provided extension '{extension.ToLower()}' is not supported."),
            };
        }

        private static string AnalyzeMediaName(string name, string startsWith, string newExtension, string oldExtension)
        {
            return name.StartsWith(startsWith) ? newExtension : oldExtension;
        }
    }
}

