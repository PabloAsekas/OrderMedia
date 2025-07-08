using Microsoft.Extensions.Options;
using OrderMedia.Handlers.Processor;
using OrderMedia.Interfaces;
using OrderMedia.Models;
using OrderMedia.Configuration;

namespace OrderMedia.UnitTests.Handlers.Processor;

[TestFixture]
public class MoveAaeProcessorHandlerTests
{
    private AutoMocker _autoMocker;
    private Mock<IIoWrapper> _ioServiceMock;
    private Mock<IAaeHelperService> _aaeHlperServiceMock;

    [SetUp]
    public void SetUp()
    {
        _autoMocker = new AutoMocker();

        _ioServiceMock = _autoMocker.GetMock<IIoWrapper>();

        _aaeHlperServiceMock = _autoMocker.GetMock<IAaeHelperService>();
        
        var classificationSettingsOptions = Options.Create(new ClassificationSettingsOptions
        {
            RenameMediaFiles = true
        });
        
        _autoMocker.Use(classificationSettingsOptions);
    }

    [Test]
    public void Process_Runs_Successfully()
    {
        // Arrange
        const string aaeName = "IMG_O0001.aae";

        var media = new Media
        {
            NameWithoutExtension = "IMG_0001",
            MediaFolder = "photos",
            NewMediaFolder = "2014-07-31",
            NewNameWithoutExtension = "2014-07-31_22-15-15_IMG_0001"
        };

        var aaeLocation = $"{media.MediaFolder}/{aaeName}";

        var newAaeName = $"{media.NewNameWithoutExtension}.aae";

        var newAaeLocation = $"{media.NewMediaFolder}/{newAaeName}";

        _aaeHlperServiceMock.Setup(x => x.GetAaeName(media.NameWithoutExtension))
            .Returns(aaeName);

        _ioServiceMock.Setup(x => x.Combine(new string[] { media.MediaFolder, aaeName }))
            .Returns(aaeLocation);

        _ioServiceMock.Setup(x => x.FileExists(aaeLocation))
            .Returns(true);

        _ioServiceMock.Setup(x => x.Combine(new string[] { media.NewMediaFolder, newAaeName }))
            .Returns(newAaeLocation);

        var sut = _autoMocker.CreateInstance<MoveAaeProcessorHandler>();

        // Act
        sut.Process(media);

        // Assert
        _ioServiceMock.Verify(x => x.MoveMedia(aaeLocation, newAaeLocation, It.IsAny<bool>()), Times.Once);
    }
}