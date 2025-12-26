using OrderMedia.Handlers.Processor;
using OrderMedia.Interfaces;
using OrderMedia.Models;

namespace OrderMedia.UnitTests.Handlers.Processor;

[TestFixture]
public class MoveMediaProcessorHandlerTests
{
    private Mock<IIoWrapper> _ioWrapperMock;
    
    [SetUp]
    public void SetUp()
    {
        _ioWrapperMock = new Mock<IIoWrapper>();
    }

    [Test]
    public void Execute_Runs_Successfully()
    {
        // Arrange
        const string originalPath = "test/photos/IMG_0001.jpg";
        const string targetDirectoryPath = "test/photos/img/2014-07-31/";
        const string targetPath = "test/photos/img/2014-07-31/2014-07-31_22-15-15_IMG_0001.jpg";
        DateTimeOffset createdDateTime = new DateTimeOffset(new DateTime(2014, 07, 31, 22, 15, 15));

        var request = new ProcessMediaRequest
        {
            Original = new Media
            {
                Path = originalPath,
                CreatedDateTime = createdDateTime,
            },
            Target = new Media
            {
                Path = targetPath,
                DirectoryPath = targetDirectoryPath,
                CreatedDateTime = createdDateTime,
            }
        };

        var sut = new MoveMediaProcessorHandler(_ioWrapperMock.Object);

        // Act
        sut.Process(request);

        // Assert
        _ioWrapperMock.Verify(x => x.CreateFolder(targetDirectoryPath), Times.Once);
        _ioWrapperMock.Verify(x => x.MoveMedia(originalPath, targetPath, It.IsAny<bool>()), Times.Once);
    }

    [Test]
    public void Execute_Runs_WithNoClassifiableMedia()
    {
        // Arrange
        const string originalPath = "test/photos/IMG_0001.jpg";
        const string targetDirectoryPath = "test/photos/img/2014-07-31/";
        const string targetPath = "test/photos/img/2014-07-31/2014-07-31_22-15-15_IMG_0001.jpg";

        var request = new ProcessMediaRequest
        {
            Original = new Media
            {
                Path = originalPath,
            },
            Target = new Media
            {
                Path = targetPath,
                DirectoryPath = targetDirectoryPath,
            }
        };

        var sut = new MoveMediaProcessorHandler(_ioWrapperMock.Object);

        // Act
        sut.Process(request);

        // Assert
        _ioWrapperMock.Verify(x => x.CreateFolder(targetDirectoryPath), Times.Never);
        _ioWrapperMock.Verify(x => x.MoveMedia(originalPath, targetPath, It.IsAny<bool>()), Times.Never);
    }
}