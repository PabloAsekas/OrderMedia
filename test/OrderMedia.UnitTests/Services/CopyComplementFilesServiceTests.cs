using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OrderMedia.Configuration;
using OrderMedia.Interfaces;
using OrderMedia.Models;
using OrderMedia.Services;

namespace OrderMedia.UnitTests.Services;

[TestFixture]
public class CopyComplementFilesServiceTests
{
    private Mock<IIoWrapper> _ioWrapperMock;
    private Mock<ILogger<CopyComplementFilesService>> _loggerMock;

    [SetUp]
    public void SetUp()
    {
        _ioWrapperMock = new Mock<IIoWrapper>();
        
        _loggerMock = new Mock<ILogger<CopyComplementFilesService>>();
    }

    [Test]
    public void CopyComplementFiles_FolderNotFound_Successfully()
    {
        // Arrange
        const string fileToApply = "/test/2014-07-31_22-22-22_IMG_0001.jpg";

        const string folderToSearch = "/Volumes/Test/";
        
        _ioWrapperMock.Setup(x => x.GetFileNameWithoutExtension(fileToApply))
            .Returns("2014-07-31_22-22-22_IMG_0001");

        IOptions<MediaPathsSettings> mediaPathsOptions = Options.Create(new MediaPathsSettings
        {
            MediaPostProcessSource = folderToSearch,
        });

        string[] combinedArray = [folderToSearch, "2014"];

        const string folderToSearchWithYear = folderToSearch + "2014";
        
        _ioWrapperMock.Setup(x => x.Combine(combinedArray))
            .Returns(folderToSearchWithYear);

        _ioWrapperMock.Setup(x => x.DirectoryExists(folderToSearchWithYear))
            .Returns(false);

        var sut = new CopyComplementFilesService(
            _ioWrapperMock.Object,
            _loggerMock.Object,
            mediaPathsOptions
            );
        
        // Act
        sut.CopyComplementFiles(fileToApply, string.Empty);
        
        // Assert
        _ioWrapperMock.Verify(x => x.GetFileNameWithoutExtension(fileToApply), Times.Once);
        _ioWrapperMock.Verify(x => x.Combine(combinedArray), Times.Once);
        _ioWrapperMock.Verify(x => x.DirectoryExists(folderToSearchWithYear), Times.Once);
        _loggerMock.Verify(x => x.Log(
            It.IsAny<LogLevel>(),
            It.IsAny<EventId>(),
            It.IsAny<It.IsAnyType>(),
            It.IsAny<Exception>(),
            It.IsAny<Func<It.IsAnyType, Exception, string>>()),
            Times.Once);
    }

    [Test]
    public void CopyComplementFiles_CopiesFile_Successfully()
    {
        // Arrange
        const string folderToApply = "/test";
        const string fileToApply = folderToApply + "/2014-07-31_22-22-22_IMG_0001.jpg";
        const string nameWithoutExtension = "2014-07-31_22-22-22_IMG_0001";
        const string folderToSearch = "/Volumes/Test";
        
        _ioWrapperMock.Setup(x => x.GetFileNameWithoutExtension(fileToApply))
            .Returns(nameWithoutExtension);

        var combinedArray = new[] { folderToSearch, "2014" };

        const string folderToSearchWithYear = folderToSearch + "/2014";
        
        _ioWrapperMock.Setup(x => x.Combine(combinedArray))
            .Returns(folderToSearchWithYear);

        _ioWrapperMock.Setup(x => x.DirectoryExists(folderToSearchWithYear))
            .Returns(true);

        var directories = new List<string>() { folderToSearchWithYear + "/2014-01-01 test", folderToSearchWithYear + "/2014-07-31 test" };

        _ioWrapperMock.Setup(x => x.GetDirectories(folderToSearchWithYear))
            .Returns(directories);

        var extensionToSearch = ".mov";
        var fileToSearch = folderToSearchWithYear + "/2014-07-31 test/" + nameWithoutExtension + extensionToSearch;

        _ioWrapperMock.Setup(x => x.FileExists(fileToSearch))
            .Returns(true);
        
        IOptions<MediaPathsSettings> mediaPaths = Options.Create(new MediaPathsSettings
        {
            MediaPostProcessPath = folderToApply,
            MediaPostProcessSource = folderToSearch,
        });

        var finalFileName = folderToApply + nameWithoutExtension + extensionToSearch;

        var sut = new CopyComplementFilesService(
            _ioWrapperMock.Object,
            _loggerMock.Object,
            mediaPaths
            );
        
        // Act
        sut.CopyComplementFiles(fileToApply, extensionToSearch);
        
        // Assert
        _ioWrapperMock.Verify(x => x.GetFileNameWithoutExtension(fileToApply), Times.Once);
        _ioWrapperMock.Verify(x => x.Combine(combinedArray), Times.Once);
        _ioWrapperMock.Verify(x => x.DirectoryExists(folderToSearchWithYear), Times.Once);
        _ioWrapperMock.Verify(x => x.GetDirectories(folderToSearchWithYear), Times.Once);
        _ioWrapperMock.Verify(x => x.FileExists(fileToSearch), Times.Once);
        _ioWrapperMock.Verify(x => x.CopyFile(fileToSearch, finalFileName), Times.Once);
    }
    
    [Test]
    public void CopyComplementFiles_FileNotFound_Successfully()
    {
        // Arrange
        const string folderToApply = "/test";
        const string fileToApply = folderToApply + "/2014-07-31_22-22-22_IMG_0001.jpg";
        const string nameWithoutExtension = "2014-07-31_22-22-22_IMG_0001";
        const string folderToSearch = "/Volumes/Test";
        
        _ioWrapperMock.Setup(x => x.GetFileNameWithoutExtension(fileToApply))
            .Returns(nameWithoutExtension);
        
        IOptions<MediaPathsSettings> mediaPaths = Options.Create(new MediaPathsSettings
        {
            MediaPostProcessSource = folderToSearch,
        });
        
        var combinedArray = new[] { folderToSearch, "2014" };

        const string folderToSearchWithYear = folderToSearch + "/2014";
        
        _ioWrapperMock.Setup(x => x.Combine(combinedArray))
            .Returns(folderToSearchWithYear);

        _ioWrapperMock.Setup(x => x.DirectoryExists(folderToSearchWithYear))
            .Returns(true);

        var directories = new List<string>() { folderToSearchWithYear + "/2014-01-01 test", folderToSearchWithYear + "/2014-07-31 test" };

        _ioWrapperMock.Setup(x => x.GetDirectories(folderToSearchWithYear))
            .Returns(directories);

        var extensionToSearch = ".mov";
        var fileToSearch = folderToSearchWithYear + "/2014-07-31 test/" + nameWithoutExtension + extensionToSearch;

        _ioWrapperMock.Setup(x => x.FileExists(fileToSearch))
            .Returns(false);

        var sut = new CopyComplementFilesService(
            _ioWrapperMock.Object,
            _loggerMock.Object,
            mediaPaths
            );
        
        // Act
        sut.CopyComplementFiles(fileToApply, extensionToSearch);
        
        // Assert
        _ioWrapperMock.Verify(x => x.GetFileNameWithoutExtension(fileToApply), Times.Once);
        _ioWrapperMock.Verify(x => x.Combine(combinedArray), Times.Once);
        _ioWrapperMock.Verify(x => x.DirectoryExists(folderToSearchWithYear), Times.Once);
        _ioWrapperMock.Verify(x => x.GetDirectories(folderToSearchWithYear), Times.Once);
        _ioWrapperMock.Verify(x => x.FileExists(fileToSearch), Times.Once);
        _ioWrapperMock.Verify(x => x.CopyFile(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
    }
}