using OrderMedia.Handlers.CreatedDate;
using OrderMedia.Interfaces;

namespace OrderMediaTests.Handlers.CreatedDate;

public class WhatsAppCreatedDateHandlerTests
{
    private AutoMocker _autoMocker;
    private Mock<IIOService> _ioServiceMock;

    [SetUp]
    public void SetUp()
    {
        _autoMocker = new AutoMocker();
        
        _ioServiceMock = _autoMocker.GetMock<IIOService>();
    }

    [Test]
    public void GetCreatedDateInfo_ReturnsData_Successfully()
    {
        // Arrange
        const string name = "PHOTO-2014-07-31-22-15-00";
        const string mediaPath = $"test/{name}.jpg";
        const string date = "2014-07-31-22-15-00";
        const string format = "yyyy-MM-dd-HH-mm-ss";

        _ioServiceMock.Setup(x => x.GetFileNameWithoutExtension(mediaPath))
            .Returns(name);
        
        var sut = _autoMocker.CreateInstance<WhatsAppCreatedDateHandler>();
        
        // Act
        var result = sut.GetCreatedDateInfo(mediaPath);
        
        // Assert
        result.Should().NotBeNull();
        result.CreatedDate.Should().BeEquivalentTo(date);
        result.Format.Should().BeEquivalentTo(format);
        _ioServiceMock.Verify(x => x.GetFileNameWithoutExtension(mediaPath), Times.Once);
    }
    
    [Test]
    public void GetCreatedDateInfo_ReturnsNull_Successfully()
    {
        // Arrange
        const string name = "photo-test";
        const string mediaPath = $"test/{name}.jpg";
        
        _ioServiceMock.Setup(x => x.GetFileNameWithoutExtension(mediaPath))
            .Returns(name);
        
        var sut = _autoMocker.CreateInstance<WhatsAppCreatedDateHandler>();
        
        // Act
        var result = sut.GetCreatedDateInfo(mediaPath);
        
        // Assert
        result.Should().BeNull();
        _ioServiceMock.Verify(x => x.GetFileNameWithoutExtension(mediaPath), Times.Once);
    }
}