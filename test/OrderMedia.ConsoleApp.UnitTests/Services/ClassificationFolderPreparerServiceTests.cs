using Microsoft.Extensions.Options;
using Moq;
using OrderMedia.ConsoleApp.Configuration;
using OrderMedia.ConsoleApp.Interfaces;
using OrderMedia.ConsoleApp.Services;
using OrderMedia.Interfaces;

namespace OrderMedia.ConsoleApp.UnitTests.Services;

public class ClassificationFolderPreparerServiceTests
{
    private Mock<IIoWrapper> _ioWrapperMock;

    [SetUp]
    public void Setup()
    {
        _ioWrapperMock = new Mock<IIoWrapper>();
    }

    [Test]
    public void Prepare_DoesNothing_WhenNoStrategiesAreProvided()
    {
        // Arrange
        var settings = Options.Create(new ClassificationSettings());

        var sut = new ClassificationFolderPreparerService(settings, [], _ioWrapperMock.Object);

        // Act
        sut.Prepare();
        
        // Assert
        _ioWrapperMock.Verify(x => x.Combine(It.IsAny<string []>()), Times.Never);
        _ioWrapperMock.Verify(x => x.CreateFolder(It.IsAny<string>()), Times.Never);
    }

    [Test]
    public void Prepare_CreatesFolder_WhenStrategiesAreProvided()
    {
        // Arrange
        const string mediaSourcePath = "/some/path/";
        const string strategyFolderName = "test";
        const string folderToCreate = $"{mediaSourcePath}{strategyFolderName}";
        
        var settings = Options.Create(new ClassificationSettings()
        {
            MediaSourcePath = mediaSourcePath
        });
        
        var strategyMock = new Mock<IClassificationMediaFolderStrategy>();
        strategyMock.Setup(x => x.GetTargetFolder())
            .Returns(strategyFolderName);

        _ioWrapperMock.Setup(x => x.Combine(new[] { mediaSourcePath, strategyFolderName }))
            .Returns(folderToCreate);
        
        var sut = new ClassificationFolderPreparerService(settings, new [] { strategyMock.Object }, _ioWrapperMock.Object);
        
        // Act
        sut.Prepare();
        
        // Assert
        strategyMock.Verify(x => x.GetTargetFolder(), Times.Once);
        _ioWrapperMock.Verify(x => x.Combine(new []{ mediaSourcePath, strategyFolderName
        }), Times.Once);
        _ioWrapperMock.Verify(x => x.CreateFolder(folderToCreate), Times.Once);
    }
}