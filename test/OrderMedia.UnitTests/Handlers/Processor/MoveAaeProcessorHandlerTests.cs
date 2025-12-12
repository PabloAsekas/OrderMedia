using Microsoft.Extensions.Options;
using OrderMedia.Handlers.Processor;
using OrderMedia.Interfaces;
using OrderMedia.Models;
using OrderMedia.Configuration;

namespace OrderMedia.UnitTests.Handlers.Processor;

[TestFixture]
public class MoveAaeProcessorHandlerTests
{
    private Mock<IIoWrapper> _ioWrapperMock;
    private Mock<IAaeHelperService> _aaeHlperServiceMock;
    private IOptions<ClassificationSettings> _classificationSettingsOptions;
    
    [SetUp]
    public void SetUp()
    {
        _ioWrapperMock = new Mock<IIoWrapper>();

        _aaeHlperServiceMock = new Mock<IAaeHelperService>();
        
        _classificationSettingsOptions = Options.Create(new ClassificationSettings
        {
            MaxMediaNameLength = 0,
            NewMediaName = string.Empty,
            OverwriteFiles = false,
            RenameMediaFiles = true,
            ReplaceLongNames = false
        });
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

        _ioWrapperMock.Setup(x => x.Combine(new string[] { media.MediaFolder, aaeName }))
            .Returns(aaeLocation);

        _ioWrapperMock.Setup(x => x.FileExists(aaeLocation))
            .Returns(true);

        _ioWrapperMock.Setup(x => x.Combine(new string[] { media.NewMediaFolder, newAaeName }))
            .Returns(newAaeLocation);

        var sut = new MoveAaeProcessorHandler(
            _ioWrapperMock.Object,
            _aaeHlperServiceMock.Object,
            _classificationSettingsOptions
            );

        // Act
        sut.Process(media);

        // Assert
        _ioWrapperMock.Verify(x => x.MoveMedia(aaeLocation, newAaeLocation, It.IsAny<bool>()), Times.Once);
    }
}