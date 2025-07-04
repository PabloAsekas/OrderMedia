using OrderMedia.Handlers.Processor;
using OrderMedia.Interfaces;
using OrderMedia.Models;

namespace OrderMedia.UnitTests.Handlers.Processor;

[TestFixture]
public class MoveLivePhotoProcessorHandlerTests
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
    public void Process_Runs_Successfully()
    {
        // Arrange
        var media = new Media()
        {
            NameWithoutExtension = "IMG_0001",
            MediaFolder = "photos",
            NewMediaFolder = "2014-07-31",
            NewNameWithoutExtension = "2014-07-31_22-15-15_IMG_0001"
        };

        string videoName = $"{media.NameWithoutExtension}.mov";

        var videoLocation = $"{media.MediaFolder}/{videoName}";

        var newVideoName = $"{media.NewNameWithoutExtension}.mov";

        var newVideoLocation = $"{media.NewMediaFolder}/{newVideoName}";

        _ioServiceMock.Setup(x => x.Combine(new string[] { media.MediaFolder, videoName }))
            .Returns(videoLocation);

        _ioServiceMock.Setup(x => x.GetExtension(It.IsAny<string>()))
            .Returns(".mov");

        _ioServiceMock.Setup(x => x.FileExists(videoLocation))
            .Returns(true);

        _ioServiceMock.Setup(x => x.Combine(new string[] { media.NewMediaFolder, newVideoName }))
            .Returns(newVideoLocation);

        var sut = _autoMocker.CreateInstance<MoveLivePhotoProcessorHandler>();

        // Act
        sut.Process(media);

        // Assert
        _ioServiceMock.Verify(x => x.MoveMedia(videoLocation, newVideoLocation), Times.Once);
    }
}