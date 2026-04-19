using OrderMedia.Handlers.CreatedDate;
using OrderMedia.Interfaces;

namespace OrderMedia.UnitTests.Handlers.CreatedDate;

public class XmpDirectoryCreatedDateHandlerTests
{
    private Mock<IImageMetadataReader> _imageMetadataReaderMock;

    [SetUp]
    public void SetUp()
    {
        _imageMetadataReaderMock = new Mock<IImageMetadataReader>();
    }
    
    [Test]
    public void GetCreatedDateInfo_ReturnsData_Successfully()
    {
        // Arrange
        const string directory = "test";
        const string name = "photo-test";
        const string mediaPath = $"{directory}/{name}.dng";
        const string date = "2014-07-31T22:15:00";
        const string xmpDate = $"{date}z";
        const string format = "yyyy-MM-ddTHH:mm:ss";

        _imageMetadataReaderMock.Setup(x => x.GetMetadataFromXmpDirectory(mediaPath, It.IsAny<string>()))
            .Returns(xmpDate);
        
        var sut = new XmpDirectoryCreatedDateHandler(_imageMetadataReaderMock.Object);
        
        // Act
        var result = sut.GetCreatedDateInfo(mediaPath);
        
        // Assert
        result.Should().NotBeNull();
        result.CreatedDate.Should().BeEquivalentTo(date);
        result.Format.Should().BeEquivalentTo(format);
        _imageMetadataReaderMock.Verify(x => x.GetMetadataFromXmpDirectory(mediaPath, It.IsAny<string>()), Times.Once);
    }
    
    [Test]
    public void GetCreatedDateInfo_ReturnsNull_WhenNoDngFile()
    {
        // Arrange
        const string directory = "test";
        const string name = "photo-test";
        const string mediaPath = $"{directory}/{name}.jpg";
        
        var sut = new XmpDirectoryCreatedDateHandler(_imageMetadataReaderMock.Object);
        
        // Act
        var result = sut.GetCreatedDateInfo(mediaPath);
        
        // Assert
        result.Should().BeNull();
        _imageMetadataReaderMock.Verify(x => x.GetMetadataFromXmpDirectory(mediaPath, It.IsAny<string>()), Times.Never);
    }
    
    [Test]
    public void GetCreatedDateInfo_ReturnsNull_WhenDngFileHasNoXmpDirectory()
    {
        // Arrange
        const string directory = "test";
        const string name = "photo-test";
        const string mediaPath = $"{directory}/{name}.dng";

        _imageMetadataReaderMock.Setup(x => x.GetMetadataFromXmpDirectory(mediaPath, It.IsAny<string>()))
            .Returns((string)null);
        
        var sut = new XmpDirectoryCreatedDateHandler(_imageMetadataReaderMock.Object);
        
        // Act
        var result = sut.GetCreatedDateInfo(mediaPath);
        
        // Assert
        result.Should().BeNull();
        _imageMetadataReaderMock.Verify(x => x.GetMetadataFromXmpDirectory(mediaPath, It.IsAny<string>()), Times.Once);
    }
}