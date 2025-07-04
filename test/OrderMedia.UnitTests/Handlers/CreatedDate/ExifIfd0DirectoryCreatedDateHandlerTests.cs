using MetadataExtractor.Formats.Exif;
using OrderMedia.Handlers.CreatedDate;
using OrderMedia.Interfaces;

namespace OrderMedia.UnitTests.Handlers.CreatedDate;

[TestFixture]
public class ExifIfd0DirectoryCreatedDateHandlerTests
{
    private AutoMocker _autoMocker;
    private Mock<IImageMetadataReader> _imageMetadataReaderMock;

    [SetUp]
    public void SetUp()
    {
        _autoMocker = new AutoMocker();
        
        _imageMetadataReaderMock = _autoMocker.GetMock<IImageMetadataReader>();
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
        
        var sut = _autoMocker.CreateInstance<ExifIfd0DirectoryCreatedDateHandler>();
        
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
            .Returns((string)null);
        
        var sut = _autoMocker.CreateInstance<ExifIfd0DirectoryCreatedDateHandler>();
        
        // Act
        var result = sut.GetCreatedDateInfo(mediaPath);
        
        // Assert
        result.Should().BeNull();
        _imageMetadataReaderMock.Verify(x =>
            x.GetMetadataByDirectoryTypeAndTag<ExifIfd0Directory>(mediaPath, ExifIfd0Directory.TagDateTime), Times.Once);
    }
}