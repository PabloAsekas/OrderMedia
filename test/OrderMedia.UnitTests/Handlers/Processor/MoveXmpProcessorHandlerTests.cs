using OrderMedia.Handlers.Processor;
using OrderMedia.Interfaces;
using OrderMedia.Models;

namespace OrderMedia.UnitTests.Handlers.Processor;

[TestFixture]
public class MoveXmpProcessorHandlerTests
{
    private Mock<IIoWrapper> _ioWrapperMock;

    [SetUp]
    public void SetUp()
    {
        _ioWrapperMock = new Mock<IIoWrapper>();
    }

    [Test]
    public void Process_Runs_Successfully_WhenXmpExists()
    {
        // Arrange
        const string originalNameWithoutExtension = "IMG_0001";
        const string originalDirectoryPath = "photos/";
        const string targetNameWithoutExtension = "2014-07-31_22-15-15_IMG_0001";
        const string targetDirectoryPath = "photos/2014-07-31/";
        const string xmpName = $"{originalNameWithoutExtension}.xmp";
        const string xmpLocation = $"{originalDirectoryPath}/{xmpName}";
        const string newXmpName = $"{targetNameWithoutExtension}.xmp";
        const string newXmpLocation = $"{targetDirectoryPath}/{newXmpName}";

        var request = new ProcessMediaRequest
        {
            Original = new Media
            {
                NameWithoutExtension = originalNameWithoutExtension,
                DirectoryPath = originalDirectoryPath
            },
            Target = new Media
            {
                NameWithoutExtension = targetNameWithoutExtension,
                DirectoryPath = targetDirectoryPath
            }
        };
        
        _ioWrapperMock.Setup(x => x.Combine(new[] { originalDirectoryPath, xmpName }))
            .Returns(xmpLocation);

        _ioWrapperMock.Setup(x => x.FileExists(xmpLocation))
            .Returns(true);

        _ioWrapperMock.Setup(x => x.Combine(new[] { targetDirectoryPath, newXmpName }))
            .Returns(newXmpLocation);

        var sut = new MoveXmpProcessorHandler(_ioWrapperMock.Object);

        // Act
        sut.Process(request);

        // Assert
        _ioWrapperMock.Verify(x => x.MoveMedia(xmpLocation, newXmpLocation, It.IsAny<bool>()), Times.Once);
    }

    [Test]
    public void Process_Runs_Successfully_WhenNoXmpExists()
    {
        // Arrange
        const string originalNameWithoutExtension = "IMG_0001";
        const string originalDirectoryPath = "photos/";
        const string targetNameWithoutExtension = "2014-07-31_22-15-15_IMG_0001";
        const string targetDirectoryPath = "photos/2014-07-31/";
        const string xmpName = $"{originalNameWithoutExtension}.xmp";
        const string xmpLocation = $"{originalDirectoryPath}/{xmpName}";

        var request = new ProcessMediaRequest
        {
            Original = new Media
            {
                NameWithoutExtension = originalNameWithoutExtension,
                DirectoryPath = originalDirectoryPath
            },
            Target = new Media
            {
                NameWithoutExtension = targetNameWithoutExtension,
                DirectoryPath = targetDirectoryPath
            }
        };
        
        _ioWrapperMock.Setup(x => x.Combine(new[] { originalDirectoryPath, xmpName }))
            .Returns(xmpLocation);

        _ioWrapperMock.Setup(x => x.FileExists(xmpLocation))
            .Returns(false);

        var sut = new MoveXmpProcessorHandler(_ioWrapperMock.Object);

        // Act
        sut.Process(request);

        // Assert
        _ioWrapperMock.Verify(x => x.MoveMedia(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>()), Times.Never);
    }
}