using OrderMedia.Handlers.Processor;
using OrderMedia.Interfaces;
using OrderMedia.Models;

namespace OrderMedia.UnitTests.Handlers.Processor;

[TestFixture]
public class MoveAaeProcessorHandlerTests
{
    private Mock<IIoWrapper> _ioWrapperMock;
    private Mock<IAaeHelperService> _aaeHlperServiceMock;
    
    [SetUp]
    public void SetUp()
    {
        _ioWrapperMock = new Mock<IIoWrapper>();

        _aaeHlperServiceMock = new Mock<IAaeHelperService>();
    }

    [Test]
    public void Process_Runs_Successfully_WhenAaeExists()
    {
        // Arrange
        const string aaeName = "IMG_O0001.aae";
        const string originalNameWithoutExtension = "IMG_0001";
        const string originalDirectoryPath = "photos/";
        const string targetNameWithoutExtension = "2014-07-31_22-15-15_IMG_0001";
        const string targetDirectoryPath = "photos/2014-07-31/";
        const string aaeLocation = $"{originalDirectoryPath}/{aaeName}";
        const string newAaeName = $"{targetNameWithoutExtension}.aae";
        const string newAaeLocation = $"{targetDirectoryPath}/{newAaeName}";

        var request = new ProcessMediaRequest()
        {
            Original = new Media
            {
                NameWithoutExtension = originalNameWithoutExtension,
                DirectoryPath = originalDirectoryPath
            },
            Target = new Media
            {
                NameWithoutExtension = targetNameWithoutExtension,
                DirectoryPath = targetDirectoryPath,
            }
        };

        _aaeHlperServiceMock.Setup(x => x.GetAaeName(originalNameWithoutExtension))
            .Returns(aaeName);
        _ioWrapperMock.Setup(x => x.Combine(new [] { originalDirectoryPath, aaeName }))
            .Returns(aaeLocation);
        _ioWrapperMock.Setup(x => x.FileExists(aaeLocation))
            .Returns(true);
        _ioWrapperMock.Setup(x => x.Combine(new string[] { targetDirectoryPath, newAaeName }))
            .Returns(newAaeLocation);

        var sut = new MoveAaeProcessorHandler(
            _ioWrapperMock.Object,
            _aaeHlperServiceMock.Object
            );

        // Act
        sut.Process(request);

        // Assert
        _ioWrapperMock.Verify(x => x.MoveMedia(aaeLocation, newAaeLocation, It.IsAny<bool>()), Times.Once);
    }
    
    [Test]
    public void Process_Runs_Successfully_WhenNoAaeExists()
    {
        const string aaeName = "IMG_O0001.aae";
        const string originalNameWithoutExtension = "IMG_0001";
        const string originalDirectoryPath = "photos/";
        const string targetNameWithoutExtension = "2014-07-31_22-15-15_IMG_0001";
        const string targetDirectoryPath = "photos/2014-07-31/";
        const string aaeLocation = $"{originalDirectoryPath}/{aaeName}";

        var request = new ProcessMediaRequest()
        {
            Original = new Media
            {
                NameWithoutExtension = originalNameWithoutExtension,
                DirectoryPath = originalDirectoryPath
            },
            Target = new Media
            {
                NameWithoutExtension = targetNameWithoutExtension,
                DirectoryPath = targetDirectoryPath,
            }
        };
        
        _aaeHlperServiceMock.Setup(x => x.GetAaeName(originalNameWithoutExtension))
            .Returns(aaeName);

        _ioWrapperMock.Setup(x => x.Combine(new [] { originalDirectoryPath, aaeName }))
            .Returns(aaeLocation);

        _ioWrapperMock.Setup(x => x.FileExists(aaeLocation))
            .Returns(false);
        
        var sut = new MoveAaeProcessorHandler(
            _ioWrapperMock.Object,
            _aaeHlperServiceMock.Object
        );

        // Act
        sut.Process(request);
        
        // Assert
        _ioWrapperMock.Verify(x => x.MoveMedia(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>()), Times.Never);
    }
}