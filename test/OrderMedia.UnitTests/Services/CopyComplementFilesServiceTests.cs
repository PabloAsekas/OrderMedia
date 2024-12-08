using Microsoft.Extensions.Logging;
using OrderMedia.Interfaces;
using OrderMedia.Services;

namespace OrderMedia.UnitTests.Services;

public class CopyComplementFilesServiceTests
{
    private AutoMocker _autoMocker;
    private Mock<IIOService> _ioServiceMock;
    private Mock<IConfigurationService> _configurationServiceMock;
    private Mock<ILogger<CopyComplementFilesService>> _loggerMock;

    [SetUp]
    public void SetUp()
    {
        _autoMocker = new AutoMocker();

        _ioServiceMock = _autoMocker.GetMock<IIOService>();

        _configurationServiceMock = _autoMocker.GetMock<IConfigurationService>();

        _loggerMock = _autoMocker.GetMock<ILogger<CopyComplementFilesService>>();
    }

    [Test]
    public void CopyComplementFiles_FolderNotFound_Successfully()
    {
        // Arrange
        var fileToApply = "/test/2014-07-31_22-22-22_IMG_0001.jpg";

        var folderToSearch = "/Volumes/Test/";
        
        _ioServiceMock.Setup(x => x.GetFileNameWithoutExtension(fileToApply))
            .Returns("2014-07-31_22-22-22_IMG_0001");

        _configurationServiceMock.Setup(x => x.GetMediaPostProcessSource())
            .Returns(folderToSearch);

        var combinedArray = new string[] { folderToSearch, "2014" };

        var folderToSearchWithYear = folderToSearch + "2014";
        
        _ioServiceMock.Setup(x => x.Combine(combinedArray))
            .Returns(folderToSearchWithYear);

        _ioServiceMock.Setup(x => x.DirectoryExists(folderToSearchWithYear))
            .Returns(false);

        var sut = _autoMocker.CreateInstance<CopyComplementFilesService>();
        
        // Act
        sut.CopyComplementFiles(fileToApply, string.Empty);
        
        // Assert
        _ioServiceMock.Verify(x => x.GetFileNameWithoutExtension(fileToApply), Times.Once);
        _ioServiceMock.Verify(x => x.Combine(combinedArray), Times.Once);
        _ioServiceMock.Verify(x => x.DirectoryExists(folderToSearchWithYear), Times.Once);
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
        var folderToApply = "/test";
        var fileToApply = folderToApply + "/2014-07-31_22-22-22_IMG_0001.jpg";
        var nameWithoutExtension = "2014-07-31_22-22-22_IMG_0001";
        var folderToSearch = "/Volumes/Test";
        
        _ioServiceMock.Setup(x => x.GetFileNameWithoutExtension(fileToApply))
            .Returns(nameWithoutExtension);

        _configurationServiceMock.Setup(x => x.GetMediaPostProcessSource())
            .Returns(folderToSearch);

        var combinedArray = new string[] { folderToSearch, "2014" };

        var folderToSearchWithYear = folderToSearch + "/2014";
        
        _ioServiceMock.Setup(x => x.Combine(combinedArray))
            .Returns(folderToSearchWithYear);

        _ioServiceMock.Setup(x => x.DirectoryExists(folderToSearchWithYear))
            .Returns(true);

        var directories = new List<string>() { folderToSearchWithYear + "/2014-01-01 test", folderToSearchWithYear + "/2014-07-31 test" };

        _ioServiceMock.Setup(x => x.GetDirectories(folderToSearchWithYear))
            .Returns(directories);

        var extensionToSearch = ".mov";
        var fileToSearch = folderToSearchWithYear + "/2014-07-31 test/" + nameWithoutExtension + extensionToSearch;

        _ioServiceMock.Setup(x => x.FileExists(fileToSearch))
            .Returns(true);

        _configurationServiceMock.Setup(x => x.GetMediaPostProcessPath())
            .Returns(folderToApply);

        var finalFileName = folderToApply + nameWithoutExtension + extensionToSearch;

        var sut = _autoMocker.CreateInstance<CopyComplementFilesService>();
        
        // Act
        sut.CopyComplementFiles(fileToApply, extensionToSearch);
        
        // Assert
        _ioServiceMock.Verify(x => x.GetFileNameWithoutExtension(fileToApply), Times.Once);
        _ioServiceMock.Verify(x => x.Combine(combinedArray), Times.Once);
        _ioServiceMock.Verify(x => x.DirectoryExists(folderToSearchWithYear), Times.Once);
        _ioServiceMock.Verify(x => x.GetDirectories(folderToSearchWithYear), Times.Once);
        _ioServiceMock.Verify(x => x.FileExists(fileToSearch), Times.Once);
        _configurationServiceMock.Verify(x => x.GetMediaPostProcessPath(), Times.Once);
        _ioServiceMock.Verify(x => x.CopyFile(fileToSearch, finalFileName), Times.Once);
    }
    
    [Test]
    public void CopyComplementFiles_FileNotFound_Successfully()
    {
        // Arrange
        var folderToApply = "/test";
        var fileToApply = folderToApply + "/2014-07-31_22-22-22_IMG_0001.jpg";
        var nameWithoutExtension = "2014-07-31_22-22-22_IMG_0001";
        var folderToSearch = "/Volumes/Test";
        
        _ioServiceMock.Setup(x => x.GetFileNameWithoutExtension(fileToApply))
            .Returns(nameWithoutExtension);

        _configurationServiceMock.Setup(x => x.GetMediaPostProcessSource())
            .Returns(folderToSearch);

        var combinedArray = new string[] { folderToSearch, "2014" };

        var folderToSearchWithYear = folderToSearch + "/2014";
        
        _ioServiceMock.Setup(x => x.Combine(combinedArray))
            .Returns(folderToSearchWithYear);

        _ioServiceMock.Setup(x => x.DirectoryExists(folderToSearchWithYear))
            .Returns(true);

        var directories = new List<string>() { folderToSearchWithYear + "/2014-01-01 test", folderToSearchWithYear + "/2014-07-31 test" };

        _ioServiceMock.Setup(x => x.GetDirectories(folderToSearchWithYear))
            .Returns(directories);

        var extensionToSearch = ".mov";
        var fileToSearch = folderToSearchWithYear + "/2014-07-31 test/" + nameWithoutExtension + extensionToSearch;

        _ioServiceMock.Setup(x => x.FileExists(fileToSearch))
            .Returns(false);

        var sut = _autoMocker.CreateInstance<CopyComplementFilesService>();
        
        // Act
        sut.CopyComplementFiles(fileToApply, extensionToSearch);
        
        // Assert
        _ioServiceMock.Verify(x => x.GetFileNameWithoutExtension(fileToApply), Times.Once);
        _ioServiceMock.Verify(x => x.Combine(combinedArray), Times.Once);
        _ioServiceMock.Verify(x => x.DirectoryExists(folderToSearchWithYear), Times.Once);
        _ioServiceMock.Verify(x => x.GetDirectories(folderToSearchWithYear), Times.Once);
        _ioServiceMock.Verify(x => x.FileExists(fileToSearch), Times.Once);
        _configurationServiceMock.Verify(x => x.GetMediaPostProcessPath(), Times.Never);
        _ioServiceMock.Verify(x => x.CopyFile(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
    }
}