using MetadataExtractor.Formats.QuickTime;
using OrderMedia.Handlers.CreatedDate;
using OrderMedia.Interfaces;

namespace OrderMedia.UnitTests.Handlers.CreatedDate;

[TestFixture]
public class QuickTimeMovieHeaderDirectoryCreatedDateHandlerTests
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
        const string date = "jue jul 31 22:15:00 2014";
        const string format = "ddd MMM dd HH:mm:ss yyyy";
        const string mediaPath = "test/test.jpg";
        
        _imageMetadataReaderMock.Setup(x =>
                x.GetMetadataByDirectoryTypeAndTag<QuickTimeMovieHeaderDirectory>(mediaPath, QuickTimeMovieHeaderDirectory.TagCreated))
            .Returns(date);
        
        var sut = new QuickTimeMovieHeaderDirectoryCreatedDateHandler(_imageMetadataReaderMock.Object);
        
        // Act
        var result = sut.GetCreatedDateInfo(mediaPath);
        
        // Assert
        result.Should().NotBeNull();
        result.CreatedDate.Should().BeEquivalentTo(date);
        result.Format.Should().BeEquivalentTo(format);
        _imageMetadataReaderMock.Verify(x =>
            x.GetMetadataByDirectoryTypeAndTag<QuickTimeMovieHeaderDirectory>(mediaPath, QuickTimeMovieHeaderDirectory.TagCreated), Times.Once);
    }
    
    [Test]
    public void GetCreatedDateInfo_ReturnsNull_Successfully()
    {
        // Arrange
        const string mediaPath = "test/test.jpg";

        _imageMetadataReaderMock.Setup(x =>
                x.GetMetadataByDirectoryTypeAndTag<QuickTimeMovieHeaderDirectory>(mediaPath, QuickTimeMovieHeaderDirectory.TagCreated))
            .Returns((string)null!);
        
        var sut = new QuickTimeMovieHeaderDirectoryCreatedDateHandler(_imageMetadataReaderMock.Object);
        
        // Act
        var result = sut.GetCreatedDateInfo(mediaPath);
        
        // Assert
        result.Should().BeNull();
        _imageMetadataReaderMock.Verify(x =>
            x.GetMetadataByDirectoryTypeAndTag<QuickTimeMovieHeaderDirectory>(mediaPath, QuickTimeMovieHeaderDirectory.TagCreated), Times.Once);
    }
}