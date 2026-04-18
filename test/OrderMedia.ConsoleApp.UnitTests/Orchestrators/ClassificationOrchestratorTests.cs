using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using OrderMedia.ConsoleApp.Configuration;
using OrderMedia.ConsoleApp.Interfaces;
using OrderMedia.ConsoleApp.Orchestrators;
using OrderMedia.Interfaces;
using OrderMedia.Interfaces.Factories;
using OrderMedia.Models;
using OrderMedia.Enums;
using OrderMedia.Interfaces.Handlers;

namespace OrderMedia.ConsoleApp.UnitTests.Orchestrators;

[TestFixture]
public class ClassificationOrchestratorTests
{
    private Mock<ILogger<ClassificationOrchestrator>> _loggerMock;
    private Mock<IIoWrapper> _ioWrapperMock;
    private IOptions<MediaExtensionsSettings> _mediaExtensionsSettingsOptions;
    private Mock<IClassificationFolderPreparer> _classificationFolderPreparerMock;
    private Mock<IMediaFactory> _mediaFactoryMock;
    private Mock<IClassificationService> _classificationServiceMock;
    private Mock<IProcessorChainFactory> _processorChainFactoryMock;
    private IOptions<ClassificationSettings> _classificationSettingsOptions;
    
    [SetUp]
    public void SetUp()
    {
        _loggerMock = new Mock<ILogger<ClassificationOrchestrator>>();
        _mediaExtensionsSettingsOptions = Options.Create(new MediaExtensionsSettings
        {
            ImageExtensions = [".jpg"],
            VideoExtensions = [".mov"]
        });
        _classificationSettingsOptions = Options.Create(new ClassificationSettings
        {
            MediaSourcePath = "/test",
            OverwriteFiles = true
        });
        _ioWrapperMock = new Mock<IIoWrapper>();
        _mediaFactoryMock = new Mock<IMediaFactory>();
        _classificationServiceMock = new Mock<IClassificationService>();
        _processorChainFactoryMock = new Mock<IProcessorChainFactory>();
        _classificationFolderPreparerMock = new Mock<IClassificationFolderPreparer>();
    }

    [Test]
    public async Task RunAsync_RunsSuccessfully_WhenThereArePhotosAndVideos()
    {
        // Arrange
        var imageFile = CreateFileInfo("/test/IMG_0001.jpg");
        var videoFile = CreateFileInfo("/test/IMG_0001.mov");

        _ioWrapperMock.Setup(x => x.GetFilesByExtensions("/test", It.Is<string[]>(e => e.SequenceEqual(new[] { ".jpg" }))))
            .Returns([imageFile]);

        _ioWrapperMock.Setup(x => x.GetFilesByExtensions("/test", It.Is<string[]>(e => e.SequenceEqual(new[] { ".mov" }))))
            .Returns([videoFile]);

        var imageMedia = new Media
        {
            Path = imageFile.FullName, 
            CreatedDateTime = DateTimeOffset.UtcNow,
            Type = MediaType.Image
        };
        var videoMedia = new Media
        {
            Path = videoFile.FullName,
            CreatedDateTime = DateTimeOffset.UtcNow,
            Type = MediaType.Video
        };

        _mediaFactoryMock.Setup(x => x.CreateMedia(imageFile.FullName))
            .Returns(imageMedia);
        _mediaFactoryMock.Setup(x => x.CreateMedia(videoFile.FullName))
            .Returns(videoMedia);

        _classificationServiceMock.Setup(x => x.Classify(imageMedia))
            .Returns(imageMedia);
        _classificationServiceMock.Setup(x => x.Classify(videoMedia))
            .Returns(videoMedia);

        var imageProcessor = new Mock<IProcessorHandler>();
        var videoProcessor = new Mock<IProcessorHandler>();

        _processorChainFactoryMock.Setup(x => x.Build(MediaType.Image))
            .Returns(imageProcessor.Object);
        _processorChainFactoryMock.Setup(x => x.Build(MediaType.Video))
            .Returns(videoProcessor.Object);

        var sut = new ClassificationOrchestrator(
            _loggerMock.Object,
            _ioWrapperMock.Object,
            _mediaExtensionsSettingsOptions,
            _classificationSettingsOptions,
            _mediaFactoryMock.Object,
            _classificationServiceMock.Object,
            _processorChainFactoryMock.Object,
            _classificationFolderPreparerMock.Object);
        
        // Act
        await sut.RunAsync(CancellationToken.None);
        
        // Assert
        _classificationFolderPreparerMock.Verify(x => x.Prepare(), Times.Once);
        
        imageProcessor.Verify(x => x.Process(It.Is<ProcessMediaRequest>(r =>
            r.Original == imageMedia &&
            r.Target == imageMedia &&
            r.OverwriteFiles)), Times.Once);

        videoProcessor.Verify(x => x.Process(It.Is<ProcessMediaRequest>(r =>
            r.Original == videoMedia &&
            r.Target == videoMedia &&
            r.OverwriteFiles)), Times.Once);

        _processorChainFactoryMock.Verify(x => x.Build(MediaType.Image), Times.Once);
        _processorChainFactoryMock.Verify(x => x.Build(MediaType.Video), Times.Once);
    }
    
    [Test]
    public async Task RunAsync_RunsSuccessfully_WhenThereAreMultiplePhotos()
    {
        // Arrange
        var imageFile1 = CreateFileInfo("/test/IMG_0001.jpg");
        var imageFile2 = CreateFileInfo("/test/IMG_0002.jpg");

        _ioWrapperMock.Setup(x => x.GetFilesByExtensions("/test", It.Is<string[]>(e => e.SequenceEqual(new[] { ".jpg" }))))
            .Returns([imageFile1, imageFile2]);

        var imageMedia1 = new Media
        {
            Path = imageFile1.FullName, 
            CreatedDateTime = DateTimeOffset.UtcNow,
            Type = MediaType.Image
        };
        var imageMedia2 = new Media
        {
            Path = imageFile2.FullName,
            CreatedDateTime = DateTimeOffset.UtcNow,
            Type = MediaType.Image
        };

        _mediaFactoryMock.Setup(x => x.CreateMedia(imageFile1.FullName))
            .Returns(imageMedia1);
        _mediaFactoryMock.Setup(x => x.CreateMedia(imageFile2.FullName))
            .Returns(imageMedia2);

        _classificationServiceMock.Setup(x => x.Classify(imageMedia1))
            .Returns(imageMedia1);
        _classificationServiceMock.Setup(x => x.Classify(imageMedia2))
            .Returns(imageMedia2);

        var imageProcessor = new Mock<IProcessorHandler>();
        
        _processorChainFactoryMock.Setup(x => x.Build(MediaType.Image))
            .Returns(imageProcessor.Object);

        var sut = new ClassificationOrchestrator(
            _loggerMock.Object,
            _ioWrapperMock.Object,
            _mediaExtensionsSettingsOptions,
            _classificationSettingsOptions,
            _mediaFactoryMock.Object,
            _classificationServiceMock.Object,
            _processorChainFactoryMock.Object,
            _classificationFolderPreparerMock.Object);
        
        // Act
        await sut.RunAsync(CancellationToken.None);
        
        // Assert
        _classificationFolderPreparerMock.Verify(x => x.Prepare(), Times.Once);
        
        imageProcessor.Verify(x => x.Process(It.Is<ProcessMediaRequest>(r =>
            r.Original == imageMedia1 &&
            r.Target == imageMedia1 &&
            r.OverwriteFiles)), Times.Once);

        imageProcessor.Verify(x => x.Process(It.Is<ProcessMediaRequest>(r =>
            r.Original == imageMedia2 &&
            r.Target == imageMedia2 &&
            r.OverwriteFiles)), Times.Once);

        _processorChainFactoryMock.Verify(x => x.Build(MediaType.Image), Times.Exactly(2));
    }

    [Test]
    public async Task RunAsync_SkipsMedia_WhenCreatedDateIsDefault()
    {
        // Arrange
        var file = CreateFileInfo(@"/test/broken.jpg");

        _ioWrapperMock
            .Setup(x => x.GetFilesByExtensions("/test", It.Is<string[]>(e => e.SequenceEqual(new[] { ".jpg" }))))
            .Returns([file]);

        _ioWrapperMock
            .Setup(x => x.GetFilesByExtensions("/test", It.Is<string[]>(e => e.SequenceEqual(new[] { ".mov" }))))
            .Returns([]);

        var unclassifiableMedia = new Media
        {
            Path = file.FullName,
            CreatedDateTime = default,
            Type = MediaType.Image,
        };
        
        _mediaFactoryMock.Setup(x => x.CreateMedia(file.FullName))
            .Returns(unclassifiableMedia);

        var sut = new ClassificationOrchestrator(
            _loggerMock.Object,
            _ioWrapperMock.Object,
            _mediaExtensionsSettingsOptions,
            _classificationSettingsOptions,
            _mediaFactoryMock.Object,
            _classificationServiceMock.Object,
            _processorChainFactoryMock.Object,
            _classificationFolderPreparerMock.Object);
        
        // Act
        await sut.RunAsync(CancellationToken.None);

        // Assert
        _classificationServiceMock.Verify(x => x.Classify(It.IsAny<Media>()), Times.Never);
        _processorChainFactoryMock.Verify(x => x.Build(It.IsAny<MediaType>()), Times.Never);
    }

    private static FileInfo CreateFileInfo(string fullName)
    {
        var fileInfo = new FileInfo(fullName);
        return fileInfo;
    }
}