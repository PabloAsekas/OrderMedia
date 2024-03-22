using System.IO;
using OrderMedia.Interfaces;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;

namespace OrderMedia.Services;

public class MetadataAggregatorService : IMetadataAggregatorService
{
    private readonly IIOService _ioService;

    public MetadataAggregatorService(IIOService ioService)
    {
        _ioService = ioService;
    }

    public Image GetImage(string imagePath)
    {
        if (!_ioService.Exists(imagePath))
        {
            throw new FileNotFoundException();
        }

        return Image.Load(imagePath);
    }

    public void SaveImage(Image image, string path)
    {
        var encoder = new JpegEncoder()
        {
            Quality = 100,
        };
        
        image.SaveAsJpeg(path, encoder);
    }
}