using Microsoft.Extensions.Options;
using OrderMedia.Configuration;
using OrderMedia.Handlers.Processor;
using OrderMedia.Interfaces;
using OrderMedia.Models;

namespace OrderMedia.UnitTests.Handlers.Processor;

[TestFixture]
public class MoveLivePhotoProcessorHandlerTests
{
    private Mock<IIoWrapper> _ioWrapperMock;
    private IOptions<ClassificationSettingsOptions> _classificationSettingsOptions;

    [SetUp]
    public void SetUp()
    {
        _ioWrapperMock = new Mock<IIoWrapper>();
        
        _classificationSettingsOptions = Options.Create(new ClassificationSettingsOptions
        {
            RenameMediaFiles = true
        });
    }

    [Test]
    public void Process_Runs_Successfully()
    {
        // Arrange
        var media = new Media
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

        _ioWrapperMock.Setup(x => x.Combine(new string[] { media.MediaFolder, videoName }))
            .Returns(videoLocation);

        _ioWrapperMock.Setup(x => x.GetExtension(It.IsAny<string>()))
            .Returns(".mov");

        _ioWrapperMock.Setup(x => x.FileExists(videoLocation))
            .Returns(true);

        _ioWrapperMock.Setup(x => x.Combine(new string[] { media.NewMediaFolder, newVideoName }))
            .Returns(newVideoLocation);

        var sut = new MoveLivePhotoProcessorHandler(
            _ioWrapperMock.Object,
            _classificationSettingsOptions
            );

        // Act
        sut.Process(media);

        // Assert
        _ioWrapperMock.Verify(x => x.MoveMedia(videoLocation, newVideoLocation, It.IsAny<bool>()), Times.Once);
    }
}