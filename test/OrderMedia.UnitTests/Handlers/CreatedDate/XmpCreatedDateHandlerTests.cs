using OrderMedia.Handlers.CreatedDate;
using OrderMedia.Interfaces;

namespace OrderMedia.UnitTests.Handlers.CreatedDate;

[TestFixture]
public class XmpCreatedDateHandlerTests
{
    private Mock<IIoWrapper> _ioWrapperMock;
    private Mock<IXmpExtractorService> _xmpExtractorServiceMock;

    [SetUp]
    public void SetUp()
    {
        _ioWrapperMock = new Mock<IIoWrapper>();
        
        _xmpExtractorServiceMock = new Mock<IXmpExtractorService>();
    }

    [Test]
    public void GetCreatedDateInfo_ReturnsData_Successfully()
    {
        // Arrange
        const string directory = "test";
        const string name = "photo-test";
        const string mediaPath = $"{directory}/{name}.jpg";
        const string xmpPath = $"{directory}/{name}.xmp";
        const string date = "2014-07-31T22:15:00";
        const string format = "yyyy-MM-ddTHH:mm:ss";

        _ioWrapperMock.Setup(x => x.GetDirectoryName(mediaPath))
            .Returns(directory);
        
        _ioWrapperMock.Setup(x => x.GetFileNameWithoutExtension(mediaPath))
            .Returns(name);

        _ioWrapperMock.Setup(x => x.Combine(new[] {directory, $"{name}.xmp"}))
            .Returns(xmpPath);

        _xmpExtractorServiceMock.Setup(x => x.GetCreatedDate(xmpPath))
            .Returns(date);
        
        var sut = new XmpCreatedDateHandler(_ioWrapperMock.Object, _xmpExtractorServiceMock.Object);
        
        // Act
        var result = sut.GetCreatedDateInfo(mediaPath);
        
        // Assert
        result.Should().NotBeNull();
        result.CreatedDate.Should().BeEquivalentTo(date);
        result.Format.Should().BeEquivalentTo(format);
        _ioWrapperMock.Verify(x => x.GetDirectoryName(mediaPath), Times.Once);
        _ioWrapperMock.Verify(x => x.GetFileNameWithoutExtension(mediaPath), Times.Once);
        _ioWrapperMock.Verify(x => x.Combine(new[] {directory, $"{name}.xmp"}), Times.Once);
        _xmpExtractorServiceMock.Verify(x => x.GetCreatedDate(xmpPath), Times.Once);
    }
    
    [Test]
    public void GetCreatedDateInfo_ReturnsNull_Successfully()
    {
        // Arrange
        const string directory = "test";
        const string name = "photo-test";
        const string mediaPath = $"{directory}/{name}.jpg";
        const string xmpPath = $"{directory}/{name}.xmp";

        _ioWrapperMock.Setup(x => x.GetDirectoryName(mediaPath))
            .Returns(directory);
        
        _ioWrapperMock.Setup(x => x.GetFileNameWithoutExtension(mediaPath))
            .Returns(name);

        _ioWrapperMock.Setup(x => x.Combine(new[] {directory, $"{name}.xmp"}))
            .Returns(xmpPath);

        _xmpExtractorServiceMock.Setup(x => x.GetCreatedDate(xmpPath))
            .Returns((string)null!);
        
        var sut = new XmpCreatedDateHandler(_ioWrapperMock.Object, _xmpExtractorServiceMock.Object);
        
        // Act
        var result = sut.GetCreatedDateInfo(mediaPath);
        
        // Assert
        result.Should().BeNull();
        _ioWrapperMock.Verify(x => x.GetDirectoryName(mediaPath), Times.Once);
        _ioWrapperMock.Verify(x => x.GetFileNameWithoutExtension(mediaPath), Times.Once);
        _ioWrapperMock.Verify(x => x.Combine(new[] {directory, $"{name}.xmp"}), Times.Once);
        _xmpExtractorServiceMock.Verify(x => x.GetCreatedDate(xmpPath), Times.Once);
    }
}