using OrderMedia.Interfaces;
using OrderMedia.Interfaces.Factories;
using OrderMedia.Models;

namespace OrderMedia.Factories;

/// <summary>
/// Media factory to create media objects based on the path.
/// </summary>
public class MediaFactory : IMediaFactory
{
    private readonly IIoWrapper _ioWrapper;
    private readonly IMediaTypeService _mediaTypeService;
    private readonly ICreatedDateExtractorService _createdDateExtractorService;

    public MediaFactory(
        IIoWrapper ioWrapper,
        IMediaTypeService mediaTypeService,
        ICreatedDateExtractorService createdDateExtractorService)
    {
        _ioWrapper = ioWrapper;
        _mediaTypeService = mediaTypeService;
        _createdDateExtractorService = createdDateExtractorService;
    }

    public Media CreateMedia(string path)
    {
        var mediaType = _mediaTypeService.GetMediaType(path);
        
        var directoryPath = _ioWrapper.GetDirectoryName(path);

        var name = _ioWrapper.GetFileName(path);

        var nameWithoutExtension = _ioWrapper.GetFileNameWithoutExtension(path);
            
        var createdDateTime = _createdDateExtractorService.GetCreatedDateTimeOffset(path);

        return new Media
        {
            Type = mediaType,
            Path = path,
            DirectoryPath = directoryPath,
            Name = name,
            NameWithoutExtension = nameWithoutExtension,
            CreatedDateTime = createdDateTime
        };
    }
}