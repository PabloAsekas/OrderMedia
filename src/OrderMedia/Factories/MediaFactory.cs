using System;
using OrderMedia.Enums;
using OrderMedia.Interfaces;
using OrderMedia.Models;

namespace OrderMedia.Factories
{
    /// <summary>
    /// Media factory to create media objects based on the path.
    /// </summary>
    public class MediaFactory : IMediaFactory
    {
        private readonly IConfigurationService _configurationService;
        private readonly IIOService _ioService;
        private readonly IRenameService _renameService;
        private readonly IMediaTypeService _mediaTypeService;
        private readonly ICreatedDateExtractorsFactory _createdDateTimeServiceFactory;

        public MediaFactory(IConfigurationService configurationService,
            IIOService ioService,
            IRenameService renameService,
            IMediaTypeService mediaTypeService,
            ICreatedDateExtractorsFactory createdDateTimeServiceFactory)
        {
            _configurationService = configurationService;
            _ioService = ioService;
            _renameService = renameService;
            _mediaTypeService = mediaTypeService;
            _createdDateTimeServiceFactory = createdDateTimeServiceFactory;
        }

        public Media CreateMedia(string path)
        {
            var mediaType = _mediaTypeService.GetMediaType(path);

            var classificationFolderName = GetClassificationFolderName(mediaType);

            var mediaFolder = _ioService.GetDirectoryName(path);

            var fullName = _ioService.GetFileName(path);

            var nameWithoutExtension = _ioService.GetFileNameWithoutExtension(path);

            var createdDateTimeService = _createdDateTimeServiceFactory.GetExtractor(mediaType);

            var createdDateTime = createdDateTimeService.GetCreatedDateTime(path);

            var createdDateTimeAsString = createdDateTime.ToString("yyyy-MM-dd");

            var newMediaFolder = _ioService.Combine(new string[] { mediaFolder, classificationFolderName, createdDateTimeAsString });

            var newName = GetNewName(fullName, createdDateTime);

            var newNameWithoutExtension = _ioService.GetFileNameWithoutExtension(newName);

            var newMediaPath = _ioService.Combine(new string[] { newMediaFolder, newName });

            return new Media()
            {
                MediaType = mediaType,
                MediaPath = path,
                MediaFolder = mediaFolder,
                Name = fullName,
                NameWithoutExtension = nameWithoutExtension,
                CreatedDateTime = createdDateTime,
                NewMediaPath = newMediaPath,
                NewMediaFolder = newMediaFolder,
                NewName = newName,
                NewNameWithoutExtension = newNameWithoutExtension,
            };
        }

        private string GetClassificationFolderName(MediaType mediaType)
        {
            return mediaType switch
            {
                MediaType.Image => _configurationService.GetImageFolderName(),
                MediaType.Raw => _configurationService.GetImageFolderName(),
                MediaType.Video => _configurationService.GetVideoFolderName(),
                MediaType.WhatsAppImage => _configurationService.GetImageFolderName(),
                MediaType.WhatsAppVideo => _configurationService.GetVideoFolderName(),
                _ => throw new FormatException($"The provided media type '{mediaType}' is not supported."),
            };
        }

        private string GetNewName(string originalName, DateTime createdDateTime)
        {
            if (_configurationService.GetRenameMediaFiles())
            {
                return _renameService.Rename(originalName, createdDateTime);
            }

            return originalName;
        }
    }
}
