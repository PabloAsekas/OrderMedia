using OrderMedia.Handlers.CreatedDate;
using OrderMedia.Interfaces;

namespace OrderMedia.UnitTests.Handlers.CreatedDate;

[TestFixture]
public class RegexCreatedDateHandlerTests
{
    private Mock<IIoWrapper> _ioWrapperMock;

    [SetUp]
    public void SetUp()
    {
        _ioWrapperMock = new Mock<IIoWrapper>();
    }

    [TestCase("PHOTO-2014-07-31-22-15-00", "2014-07-31-22-15-00", "[0-9]{4}-(0[1-9]|1[0-2])-(0[1-9]|[1-2][0-9]|3[0-1])-(0[0-9]|[1-2][0-9])-([0-5][0-9])-([0-5][0-9])", "yyyy-MM-dd-HH-mm-ss")]
    [TestCase("IMG_20140731_221500", "20140731_221500", "[0-9]{4}(0[1-9]|1[0-2])(0[1-9]|[1-2][0-9]|3[0-1])_(0[0-9]|[1-2][0-9])([0-5][0-9])([0-5][0-9])", "yyyyMMdd_HHmmss")]
    [TestCase("24-08-03 18-29-44 1005", "24-08-03 18-29-44", "[0-9]{2}-(0[1-9]|1[0-2])-(0[1-9]|[1-2][0-9]|3[0-1]) (0[0-9]|[1-2][0-9])-([0-5][0-9])-([0-5][0-9])", "yy-MM-dd HH-mm-ss")]
    public void GetCreatedDateInfo_ReturnsData_Successfully(string name, string date, string pattern, string format)
    {
        // Arrange
        var mediaPath = $"test/{name}.jpg";

        _ioWrapperMock.Setup(x => x.GetFileNameWithoutExtension(mediaPath))
            .Returns(name);
        
        var sut = new RegexCreatedDateHandler(_ioWrapperMock.Object, pattern, format);
        
        // Act
        var result = sut.GetCreatedDateInfo(mediaPath);
        
        // Assert
        result.Should().NotBeNull();
        result.CreatedDate.Should().BeEquivalentTo(date);
        result.Format.Should().BeEquivalentTo(format);
        _ioWrapperMock.Verify(x => x.GetFileNameWithoutExtension(mediaPath), Times.Once);
    }
    
    [Test]
    public void GetCreatedDateInfo_ReturnsNull_Successfully()
    {
        // Arrange
        const string name = "photo-test";
        const string mediaPath = $"test/{name}.jpg";
        
        _ioWrapperMock.Setup(x => x.GetFileNameWithoutExtension(mediaPath))
            .Returns(name);
        
        var sut = new RegexCreatedDateHandler(_ioWrapperMock.Object, "", "");
        
        // Act
        var result = sut.GetCreatedDateInfo(mediaPath);
        
        // Assert
        result.Should().BeNull();
        _ioWrapperMock.Verify(x => x.GetFileNameWithoutExtension(mediaPath), Times.Once);
    }
}