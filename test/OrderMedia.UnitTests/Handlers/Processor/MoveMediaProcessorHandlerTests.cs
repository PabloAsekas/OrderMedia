using Microsoft.Extensions.Options;
using OrderMedia.Configuration;
using OrderMedia.Handlers.Processor;
using OrderMedia.Interfaces;
using OrderMedia.Models;

namespace OrderMedia.UnitTests.Handlers.Processor;

[TestFixture]
public class MoveMediaProcessorHandlerTests
{
    private Mock<IIoWrapper> _ioWrapperMock;
    private IOptions<ClassificationSettingsOptions> _classificationSettingsOptions;
    
    [SetUp]
    public void SetUp()
    {

        _ioWrapperMock = new Mock<IIoWrapper>();
        
        _classificationSettingsOptions = Options.Create(new ClassificationSettingsOptions
        {
            MaxMediaNameLength = 0,
            NewMediaName = string.Empty,
            OverwriteFiles = false,
            RenameMediaFiles = true,
            ReplaceLongNames = false
        });
    }

    [Test]
    public void Execute_Runs_Successfully()
    {
        // Arrange
        var media = new Media
        {
            NewMediaFolder = "/2014-07-31/",
            MediaPath = "test/photos/IMG_0001.jpg",
            NewMediaPath = "test/photos/img/2014-07-31/2014-07-31_22-15-15_IMG_0001.jpg",
            CreatedDateTimeOffset = new DateTimeOffset(new DateTime(2014, 07, 31, 22, 15, 15)),
        };

        var sut = new MoveMediaProcessorHandler(
            _ioWrapperMock.Object,
            _classificationSettingsOptions
            );

        // Act
        sut.Process(media);

        // Assert
        _ioWrapperMock.Verify(x => x.CreateFolder(media.NewMediaFolder), Times.Once);
        _ioWrapperMock.Verify(x => x.MoveMedia(media.MediaPath, media.NewMediaPath, It.IsAny<bool>()), Times.Once);
    }

    [Test]
    public void Execute_Runs_WithNoClassifiableMedia()
    {
        // Arrange
        var media = new Media
        {
            NewMediaFolder = "/2014-07-31/",
            MediaPath = "test/photos/IMG_0001.jpg",
            NewMediaPath = "test/photos/img/2014-07-31/2014-07-31_22-15-15_IMG_0001.jpg",
        };

        var sut = new MoveMediaProcessorHandler(
            _ioWrapperMock.Object,
            _classificationSettingsOptions
            );

        // Act
        sut.Process(media);

        // Assert
        _ioWrapperMock.Verify(x => x.CreateFolder(media.NewMediaFolder), Times.Never);
        _ioWrapperMock.Verify(x => x.MoveMedia(media.MediaPath, media.NewMediaPath, It.IsAny<bool>()), Times.Never);
    }
}