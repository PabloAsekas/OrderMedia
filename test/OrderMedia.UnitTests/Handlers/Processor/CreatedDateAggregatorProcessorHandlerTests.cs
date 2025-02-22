using OrderMedia.Handlers.Processor;
using OrderMedia.Interfaces;
using OrderMedia.Models;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Metadata.Profiles.Exif;
using SixLabors.ImageSharp.PixelFormats;

namespace OrderMedia.UnitTests.Handlers.Processor;

public class CreatedDateAggregatorProcessorHandlerTests
{
    private AutoMocker _autoMocker;
    private Mock<IMetadataAggregatorService> _metadataAggregatorServiceMock;
    
    [SetUp]
    public void SetUp()
    {
        _autoMocker = new AutoMocker();
        
        _metadataAggregatorServiceMock = _autoMocker.GetMock<IMetadataAggregatorService>();
    }

    [TestCase]
    public void Process_Runs_Successfully()
    {
        // Arrange
        var media = new Media()
        {
            CreatedDateTimeOffset = new DateTime(2014, 07, 31, 22, 15, 15),
            NewMediaPath = "/photos/2014-07-31/2014-07-31_22-15-15_IMG_0001",
        };

        var image = new Image<Rgba32>(100, 100);
        
        _metadataAggregatorServiceMock.Setup(x => x.GetImage(It.IsAny<string>()))
            .Returns(image);
        
        var sut = _autoMocker.CreateInstance<CreatedDateAggregatorProcessorHandler>();

        // Act
        sut.Process(media);
        
        // Assert
        var mediaDateTime = media.CreatedDateTimeOffset.ToString("yyyy:MM:dd HH:mm:ss");
        var offset = "+01:00";
        
        _metadataAggregatorServiceMock.Verify(x => x.GetImage(media.NewMediaPath), Times.Once);
        image.Metadata.ExifProfile.Should().NotBeNull();
        TryGetValue(image, ExifTag.DateTimeOriginal).Should().Be(mediaDateTime);
        TryGetValue(image, ExifTag.DateTime).Should().Be(mediaDateTime);
        TryGetValue(image, ExifTag.DateTimeDigitized).Should().Be(mediaDateTime);
        TryGetValue(image, ExifTag.OffsetTime).Should().Be(offset);
        TryGetValue(image, ExifTag.OffsetTimeOriginal).Should().Be(offset);
        TryGetValue(image, ExifTag.OffsetTimeDigitized).Should().Be(offset);
    }
    
    private static string TryGetValue(Image image, ExifTag<string> tag)
    {
        image.Metadata.ExifProfile.TryGetValue(tag, out var tagValue);
        
        return tagValue.Value;
    }
}