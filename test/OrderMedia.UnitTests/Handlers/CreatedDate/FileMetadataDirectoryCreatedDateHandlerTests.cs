using MetadataExtractor.Formats.FileSystem;
using OrderMedia.Handlers.CreatedDate;
using OrderMedia.Interfaces;

namespace OrderMedia.UnitTests.Handlers.CreatedDate;

[TestFixture]
public class FileMetadataDirectoryCreatedDateHandlerTests
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
        const string date = "jue jul 31 22:15:00 +00:00 2014";
        const string format = "ddd MMM dd HH:mm:ss zzz yyyy";
        const string mediaPath = "test/test.jpg";
        
        _imageMetadataReaderMock.Setup(x =>
                x.GetMetadataByDirectoryTypeAndTag<FileMetadataDirectory>(mediaPath, FileMetadataDirectory.TagFileModifiedDate))
            .Returns(date);
        
        var sut = _autoMocker.CreateInstance<FileMetadataDirectoryCreatedDateHandler>();
        
        // Act
        var result = sut.GetCreatedDateInfo(mediaPath);
        
        // Assert
        result.Should().NotBeNull();
        result.CreatedDate.Should().BeEquivalentTo(date);
        result.Format.Should().BeEquivalentTo(format);
        _imageMetadataReaderMock.Verify(x =>
            x.GetMetadataByDirectoryTypeAndTag<FileMetadataDirectory>(mediaPath, FileMetadataDirectory.TagFileModifiedDate), Times.Once);
    }
    
    [Test]
    public void GetCreatedDateInfo_ReturnsNull_Successfully()
    {
        // Arrange
        const string mediaPath = "test/test.jpg";

        _imageMetadataReaderMock.Setup(x =>
                x.GetMetadataByDirectoryTypeAndTag<FileMetadataDirectory>(mediaPath, FileMetadataDirectory.TagFileModifiedDate))
            .Returns((string)null);
        
        var sut = _autoMocker.CreateInstance<FileMetadataDirectoryCreatedDateHandler>();
        
        // Act
        var result = sut.GetCreatedDateInfo(mediaPath);
        
        // Assert
        result.Should().BeNull();
        _imageMetadataReaderMock.Verify(x =>
            x.GetMetadataByDirectoryTypeAndTag<FileMetadataDirectory>(mediaPath, FileMetadataDirectory.TagFileModifiedDate), Times.Once);
    }
}