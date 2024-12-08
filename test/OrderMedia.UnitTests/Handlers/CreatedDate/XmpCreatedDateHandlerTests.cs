using OrderMedia.Handlers.CreatedDate;
using OrderMedia.Interfaces;

namespace OrderMedia.UnitTests.Handlers.CreatedDate;

public class XmpCreatedDateHandlerTests
{
    private AutoMocker _autoMocker;
    private Mock<IIOService> _ioServiceMock;
    private Mock<IXmpExtractorService> _xmpExtractorServiceMock;

    [SetUp]
    public void SetUp()
    {
        _autoMocker = new AutoMocker();
        
        _ioServiceMock = _autoMocker.GetMock<IIOService>();
        
        _xmpExtractorServiceMock = _autoMocker.GetMock<IXmpExtractorService>();
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

        _ioServiceMock.Setup(x => x.GetDirectoryName(mediaPath))
            .Returns(directory);
        
        _ioServiceMock.Setup(x => x.GetFileNameWithoutExtension(mediaPath))
            .Returns(name);

        _ioServiceMock.Setup(x => x.Combine(new[] {directory, $"{name}.xmp"}))
            .Returns(xmpPath);

        _xmpExtractorServiceMock.Setup(x => x.GetCreatedDate(xmpPath))
            .Returns(date);
        
        var sut = _autoMocker.CreateInstance<XmpCreatedDateHandler>();
        
        // Act
        var result = sut.GetCreatedDateInfo(mediaPath);
        
        // Assert
        result.Should().NotBeNull();
        result.CreatedDate.Should().BeEquivalentTo(date);
        result.Format.Should().BeEquivalentTo(format);
        _ioServiceMock.Verify(x => x.GetDirectoryName(mediaPath), Times.Once);
        _ioServiceMock.Verify(x => x.GetFileNameWithoutExtension(mediaPath), Times.Once);
        _ioServiceMock.Verify(x => x.Combine(new[] {directory, $"{name}.xmp"}), Times.Once);
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

        _ioServiceMock.Setup(x => x.GetDirectoryName(mediaPath))
            .Returns(directory);
        
        _ioServiceMock.Setup(x => x.GetFileNameWithoutExtension(mediaPath))
            .Returns(name);

        _ioServiceMock.Setup(x => x.Combine(new[] {directory, $"{name}.xmp"}))
            .Returns(xmpPath);

        _xmpExtractorServiceMock.Setup(x => x.GetCreatedDate(xmpPath))
            .Returns((string)null);
        
        var sut = _autoMocker.CreateInstance<XmpCreatedDateHandler>();
        
        // Act
        var result = sut.GetCreatedDateInfo(mediaPath);
        
        // Assert
        result.Should().BeNull();
        _ioServiceMock.Verify(x => x.GetDirectoryName(mediaPath), Times.Once);
        _ioServiceMock.Verify(x => x.GetFileNameWithoutExtension(mediaPath), Times.Once);
        _ioServiceMock.Verify(x => x.Combine(new[] {directory, $"{name}.xmp"}), Times.Once);
        _xmpExtractorServiceMock.Verify(x => x.GetCreatedDate(xmpPath), Times.Once);
    }
}