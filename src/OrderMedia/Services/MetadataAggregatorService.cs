using System.IO;
using OrderMedia.Interfaces;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;

namespace OrderMedia.Services;

public class MetadataAggregatorService : IMetadataAggregatorService
{
    private readonly IIoWrapper _ioWrapper;

    public MetadataAggregatorService(IIoWrapper ioWrapper)
    {
        _ioWrapper = ioWrapper;
    }

    public Image GetImage(string imagePath)
    {
        if (!_ioWrapper.FileExists(imagePath))
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