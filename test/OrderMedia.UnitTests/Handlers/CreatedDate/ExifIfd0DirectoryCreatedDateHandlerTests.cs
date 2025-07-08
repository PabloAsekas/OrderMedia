using MetadataExtractor.Formats.Exif;
using OrderMedia.Handlers.CreatedDate;
using OrderMedia.Interfaces;

namespace OrderMedia.UnitTests.Handlers.CreatedDate;

[TestFixture]
public class ExifIfd0DirectoryCreatedDateHandlerTests
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
        const string date = "2014:07:31 22:15:00";
        const string format = "yyyy:MM:dd HH:mm:ss";
        const string mediaPath = "test/test.jpg";
        
        _imageMetadataReaderMock.Setup(x =>
                x.GetMetadataByDirectoryTypeAndTag<ExifIfd0Directory>(mediaPath, ExifIfd0Directory.TagDateTime))
            .Returns(date);
        
        var sut = new ExifIfd0DirectoryCreatedDateHandler(_imageMetadataReaderMock.Object);
        
        // Act
        var result = sut.GetCreatedDateInfo(mediaPath);
        
        // Assert
        result.Should().NotBeNull();
        result.CreatedDate.Should().BeEquivalentTo(date);
        result.Format.Should().BeEquivalentTo(format);
        _imageMetadataReaderMock.Verify(x =>
            x.GetMetadataByDirectoryTypeAndTag<ExifIfd0Directory>(mediaPath, ExifIfd0Directory.TagDateTime), Times.Once);
    }
    
    [Test]
    public void GetCreatedDateInfo_ReturnsNull_Successfully()
    {
        // Arrange
        const string mediaPath = "test/test.jpg";
        
        _imageMetadataReaderMock.Setup(x =>
                x.GetMetadataByDirectoryTypeAndTag<ExifIfd0Directory>(mediaPath, ExifIfd0Directory.TagDateTime))
            .Returns((string)null!);
        
        var sut = new ExifIfd0DirectoryCreatedDateHandler(_imageMetadataReaderMock.Object);
        
        // Act
        var result = sut.GetCreatedDateInfo(mediaPath);
        
        // Assert
        result.Should().BeNull();
        _imageMetadataReaderMock.Verify(x =>
            x.GetMetadataByDirectoryTypeAndTag<ExifIfd0Directory>(mediaPath, ExifIfd0Directory.TagDateTime), Times.Once);
    }
}