using OrderMedia.Interfaces;
using OrderMedia.Models;
using OrderMedia.Services;

namespace OrderMedia.UnitTests.Services;

[TestFixture]
public class CreatedDateExtractorServiceTests
{
    private Mock<IMetadataExtractorService> _metadataExtractorServiceMock;

    [SetUp]
    public void SetUp()
    {
        _metadataExtractorServiceMock = new Mock<IMetadataExtractorService>();
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

        var sut = new CreatedDateExtractorService(_metadataExtractorServiceMock.Object);
        
        // Act
        var result = sut.GetCreatedDateTimeOffset(mediaPath);
        
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
            .Returns((CreatedDateInfo)null!);

        var sut = new CreatedDateExtractorService(_metadataExtractorServiceMock.Object);
        
        // Act
        var result = sut.GetCreatedDateTimeOffset(mediaPath);
        
        // Assert
        result.Should().BeSameDateAs(defaultDate);
        _metadataExtractorServiceMock.Verify(x => x.GetCreatedDate(mediaPath), Times.Once);
    }
}