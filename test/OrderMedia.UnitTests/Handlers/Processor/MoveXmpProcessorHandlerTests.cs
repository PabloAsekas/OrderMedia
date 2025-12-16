using Microsoft.Extensions.Options;
using OrderMedia.Configuration;
using OrderMedia.Handlers.Processor;
using OrderMedia.Interfaces;
using OrderMedia.Models;

namespace OrderMedia.UnitTests.Handlers.Processor;

[TestFixture]
public class MoveXmpProcessorHandlerTests
{
    private Mock<IIoWrapper> _ioWrapperMock;
    private IOptions<ClassificationSettings> _classificationSettingsOptions;

    [SetUp]
    public void SetUp()
    {
        _ioWrapperMock = new Mock<IIoWrapper>();
        
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
    public void Execute_Runs_Successfully()
    {
        // Arrange
        const string xmpName = "IMG_0001.xmp";

        var media = new Media
        {
            NameWithoutExtension = "IMG_0001",
            DirectoryPath = "photos",
            NewMediaFolder = "2014-07-31",
            NewNameWithoutExtension = "2014-07-31_22-15-15_IMG_0001"
        };

        var xmpLocation = $"{media.DirectoryPath}/{xmpName}";

        var newXmpName = $"{media.NewNameWithoutExtension}.xmp";

        var newXmpLocation = $"{media.NewMediaFolder}/{newXmpName}";
        
        _ioWrapperMock.Setup(x => x.Combine(new[] { media.DirectoryPath, xmpName }))
            .Returns(xmpLocation);

        _ioWrapperMock.Setup(x => x.FileExists(xmpLocation))
            .Returns(true);

        _ioWrapperMock.Setup(x => x.Combine(new[] { media.NewMediaFolder, newXmpName }))
            .Returns(newXmpLocation);

        var sut = new MoveXmpProcessorHandler(
            _ioWrapperMock.Object,
            _classificationSettingsOptions);

        // Act
        sut.Process(media);

        // Assert
        _ioWrapperMock.Verify(x => x.MoveMedia(xmpLocation, newXmpLocation, It.IsAny<bool>()), Times.Once);
    }
}