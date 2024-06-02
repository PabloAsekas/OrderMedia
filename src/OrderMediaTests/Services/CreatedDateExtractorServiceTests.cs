using OrderMedia.Interfaces;
using OrderMedia.Models;
using OrderMedia.Services;

namespace OrderMediaTests.Services;

public class CreatedDateExtractorServiceTests
{
    private AutoMocker _autoMocker;
    private Mock<IMetadataExtractorService> _metadataExtractorServiceMock;

    [SetUp]
    public void SetUp()
    {
        _autoMocker = new AutoMocker();

        _metadataExtractorServiceMock = _autoMocker.GetMock<IMetadataExtractorService>();
    }

    [Test]
    public void GetCreatedDateTime_ReturnsDateTime_Successfully()
    {
        // Arrange
        const string mediaPath = "test/test.jpg";
        var createdDateInfo = new CreatedDateInfo()
        {
            CreatedDate = "2014:07:31 22:15:00",
            Format = "yyyy:MM:dd HH:mm:ss",
        };

        _metadataExtractorServiceMock.Setup(x => x.GetCreatedDate(mediaPath))
            .Returns(createdDateInfo);

        var sut = _autoMocker.CreateInstance<CreatedDateExtractorService>();
        
        // Act
        var result = sut.GetCreatedDateTime(mediaPath);
        
        // Assert
        result.ToString(createdDateInfo.Format).Should().BeEquivalentTo(createdDateInfo.CreatedDate);
        _metadataExtractorServiceMock.Verify(x => x.GetCreatedDate(mediaPath), Times.Once);
    }
    
    [Test]
    public void GetCreatedDateTime_ReturnsDefaultDateTime_Successfully()
    {
        // Arrange
        const string mediaPath = "test/test.jpg";
        var defaultDate = default(DateTime);

        _metadataExtractorServiceMock.Setup(x => x.GetCreatedDate(mediaPath))
            .Returns((CreatedDateInfo)null);

        var sut = _autoMocker.CreateInstance<CreatedDateExtractorService>();
        
        // Act
        var result = sut.GetCreatedDateTime(mediaPath);
        
        // Assert
        result.Should().BeSameDateAs(defaultDate);
        _metadataExtractorServiceMock.Verify(x => x.GetCreatedDate(mediaPath), Times.Once);
    }
}