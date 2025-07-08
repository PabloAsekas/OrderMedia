using Microsoft.Extensions.Options;
using OrderMedia.Configuration;
using OrderMedia.Handlers.Processor;
using OrderMedia.Interfaces;
using OrderMedia.Models;

namespace OrderMedia.UnitTests.Handlers.Processor;

[TestFixture]
public class MoveMediaProcessorHandlerTests
{
    private AutoMocker _autoMocker;
    private Mock<IIoWrapper> _ioServiceMock;

    [SetUp]
    public void SetUp()
    {
        _autoMocker = new AutoMocker();

        _ioServiceMock = _autoMocker.GetMock<IIoWrapper>();
        
        var classificationSettingsOptions = Options.Create(new ClassificationSettingsOptions
        {
            RenameMediaFiles = true
        });
        
        _autoMocker.Use(classificationSettingsOptions);
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

        var sut = _autoMocker.CreateInstance<MoveMediaProcessorHandler>();

        // Act
        sut.Process(media);

        // Assert
        _ioServiceMock.Verify(x => x.CreateFolder(media.NewMediaFolder), Times.Once);
        _ioServiceMock.Verify(x => x.MoveMedia(media.MediaPath, media.NewMediaPath, It.IsAny<bool>()), Times.Once);
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

        var sut = _autoMocker.CreateInstance<MoveMediaProcessorHandler>();

        // Act
        sut.Process(media);

        // Assert
        _ioServiceMock.Verify(x => x.CreateFolder(media.NewMediaFolder), Times.Never);
        _ioServiceMock.Verify(x => x.MoveMedia(media.MediaPath, media.NewMediaPath, It.IsAny<bool>()), Times.Never);
    }
}