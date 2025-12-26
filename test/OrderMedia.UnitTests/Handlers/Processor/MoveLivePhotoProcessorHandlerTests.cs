using OrderMedia.Handlers.Processor;
using OrderMedia.Interfaces;
using OrderMedia.Models;

namespace OrderMedia.UnitTests.Handlers.Processor;

[TestFixture]
public class MoveLivePhotoProcessorHandlerTests
{
    private Mock<IIoWrapper> _ioWrapperMock;

    [SetUp]
    public void SetUp()
    {
        _ioWrapperMock = new Mock<IIoWrapper>();
    }

    [Test]
    public void Process_Runs_Successfully_WhenLivePhotoExists()
    {
        // Arrange
        const string originalNameWithoutExtension = "IMG_0001";
        const string originalDirectoryPath = "photos/";
        const string targetNameWithoutExtension = "2014-07-31_22-15-15_IMG_0001";
        const string targetDirectoryPath = "photos/2014-07-31";
        const string videoName = $"{originalNameWithoutExtension}.mov";
        const string videoLocation = $"{originalDirectoryPath}/{videoName}";
        const string newVideoName = $"{targetNameWithoutExtension}.mov";
        const string newVideoLocation = $"{targetDirectoryPath}/{newVideoName}";
        
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

        _ioWrapperMock.Setup(x => x.Combine(new [] { originalDirectoryPath, videoName }))
            .Returns(videoLocation);
        _ioWrapperMock.Setup(x => x.GetExtension(It.IsAny<string>()))
            .Returns(".mov");
        _ioWrapperMock.Setup(x => x.FileExists(videoLocation))
            .Returns(true);
        _ioWrapperMock.Setup(x => x.Combine(new string[] { targetDirectoryPath, newVideoName }))
            .Returns(newVideoLocation);

        var sut = new MoveLivePhotoProcessorHandler(_ioWrapperMock.Object);

        // Act
        sut.Process(request);

        // Assert
        _ioWrapperMock.Verify(x => x.MoveMedia(videoLocation, newVideoLocation, It.IsAny<bool>()), Times.Once);
    }
    
    [Test]
    public void Process_Runs_Successfully_WhenNoLivePhotoExists()
    {
        // Arrange
        const string originalNameWithoutExtension = "IMG_0001";
        const string originalDirectoryPath = "photos/";
        const string targetNameWithoutExtension = "2014-07-31_22-15-15_IMG_0001";
        const string targetDirectoryPath = "photos/2014-07-31";
        const string videoName = $"{originalNameWithoutExtension}.mov";
        const string videoLocation = $"{originalDirectoryPath}/{videoName}";
        
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

        _ioWrapperMock.Setup(x => x.Combine(new [] { originalDirectoryPath, videoName }))
            .Returns(videoLocation);
        _ioWrapperMock.Setup(x => x.GetExtension(It.IsAny<string>()))
            .Returns(".mov");
        _ioWrapperMock.Setup(x => x.FileExists(videoLocation))
            .Returns(false);

        var sut = new MoveLivePhotoProcessorHandler(_ioWrapperMock.Object);

        // Act
        sut.Process(request);

        // Assert
        _ioWrapperMock.Verify(x => x.MoveMedia(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>()), Times.Never);
    }
}