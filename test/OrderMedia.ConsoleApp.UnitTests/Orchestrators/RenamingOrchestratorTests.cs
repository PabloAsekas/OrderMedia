using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using OrderMedia.ConsoleApp.Configuration;
using OrderMedia.ConsoleApp.Interfaces;
using OrderMedia.ConsoleApp.Orchestrators;
using OrderMedia.Enums;
using OrderMedia.Interfaces;
using OrderMedia.Interfaces.Factories;
using OrderMedia.Interfaces.Handlers;
using OrderMedia.Models;

namespace OrderMedia.ConsoleApp.UnitTests.Orchestrators;

[TestFixture]
public class RenamingOrchestratorTests
{
    private Mock<ILogger<RenamingOrchestrator>> _loggerMock;
    private IOptions<MediaExtensionsSettings> _mediaExtensionsSettingsOptions;
    private IOptions<RenamingSettings> _renamingSettingsOptions;
    private Mock<IIoWrapper> _ioWrapperMock;
    private Mock<IMediaFactory> _mediaFactoryMock;
    private Mock<IRenamingService> _renamingServiceMock;
    private Mock<IProcessorChainFactory> _processorChainFactoryMock;
    private Mock<IRenamingValidatorService> _renamingValidatorServiceMock;
    
    [SetUp]
    public void Setup()
    {
        _loggerMock = new Mock<ILogger<RenamingOrchestrator>>();
        _mediaExtensionsSettingsOptions = Options.Create(new MediaExtensionsSettings
        {
            ImageExtensions = [".jpg"],
            VideoExtensions = [".mov"]
        });
        _renamingSettingsOptions = Options.Create(new RenamingSettings()
        {
            MediaSourcePath = "/test",
        });
        _ioWrapperMock = new Mock<IIoWrapper>();
        _mediaFactoryMock = new Mock<IMediaFactory>();
        _renamingServiceMock = new Mock<IRenamingService>();
        _processorChainFactoryMock = new Mock<IProcessorChainFactory>();
        _renamingValidatorServiceMock = new Mock<IRenamingValidatorService>();
    }
    
    [Test]
    public async Task RunAsync_RunsSuccessfully_WhenThereArePhotosAndVideos()
    {
        // Arrange
        var imageFile = CreateFileInfo("/test/IMG_0001.jpg");
        var videoFile = CreateFileInfo("/test/IMG_0001.mov");

        _ioWrapperMock.Setup(x => x.GetAllFilesByExtensions("/test", It.Is<string[]>(e => e.SequenceEqual(new[] { ".jpg" }))))
            .Returns([imageFile]);

        _ioWrapperMock.Setup(x => x.GetAllFilesByExtensions("/test", It.Is<string[]>(e => e.SequenceEqual(new[] { ".mov" }))))
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
        
        _renamingValidatorServiceMock.Setup(x => x.ValidateMedia(It.IsAny<Media>()))
            .Returns(true);

        _renamingServiceMock.Setup(x => x.Rename(imageMedia))
            .Returns(imageMedia);
        _renamingServiceMock.Setup(x => x.Rename(videoMedia))
            .Returns(videoMedia);

        var imageProcessor = new Mock<IProcessorHandler>();
        var videoProcessor = new Mock<IProcessorHandler>();

        _processorChainFactoryMock.Setup(x => x.Build(MediaType.Image))
            .Returns(imageProcessor.Object);
        _processorChainFactoryMock.Setup(x => x.Build(MediaType.Video))
            .Returns(videoProcessor.Object);

        var sut = new RenamingOrchestrator(
            _loggerMock.Object,
            _mediaExtensionsSettingsOptions,
            _renamingSettingsOptions,
            _ioWrapperMock.Object,
            _mediaFactoryMock.Object,
            _renamingServiceMock.Object,
            _processorChainFactoryMock.Object,
            _renamingValidatorServiceMock.Object);
        
        // Act
        await sut.RunAsync(CancellationToken.None);
        
        // Assert
        _renamingValidatorServiceMock.Verify(x => x.ValidateMedia(It.IsAny<Media>()), Times.Exactly(2));
        
        imageProcessor.Verify(x => x.Process(It.Is<ProcessMediaRequest>(r =>
            r.Original == imageMedia &&
            r.Target == imageMedia)), Times.Once);

        videoProcessor.Verify(x => x.Process(It.Is<ProcessMediaRequest>(r =>
            r.Original == videoMedia &&
            r.Target == videoMedia)), Times.Once);

        _processorChainFactoryMock.Verify(x => x.Build(MediaType.Image), Times.Once);
        _processorChainFactoryMock.Verify(x => x.Build(MediaType.Video), Times.Once);
    }
    
    [Test]
    public async Task RunAsync_RunsSuccessfully_WhenThereAreMultiplePhotos()
    {
        var imageFile1 = CreateFileInfo("/test/IMG_0001.jpg");
        var imageFile2 = CreateFileInfo("/test/IMG_0002.jpg");

        _ioWrapperMock.Setup(x => x.GetAllFilesByExtensions("/test", It.Is<string[]>(e => e.SequenceEqual(new[] { ".jpg" }))))
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
        
        _renamingValidatorServiceMock.Setup(x => x.ValidateMedia(It.IsAny<Media>()))
            .Returns(true);

        _renamingServiceMock.Setup(x => x.Rename(imageMedia1))
            .Returns(imageMedia1);
        _renamingServiceMock.Setup(x => x.Rename(imageMedia2))
            .Returns(imageMedia2);

        var imageProcessor = new Mock<IProcessorHandler>();

        _processorChainFactoryMock.Setup(x => x.Build(MediaType.Image))
            .Returns(imageProcessor.Object);

        var sut = new RenamingOrchestrator(
            _loggerMock.Object,
            _mediaExtensionsSettingsOptions,
            _renamingSettingsOptions,
            _ioWrapperMock.Object,
            _mediaFactoryMock.Object,
            _renamingServiceMock.Object,
            _processorChainFactoryMock.Object,
            _renamingValidatorServiceMock.Object);
        
        await sut.RunAsync(CancellationToken.None);
        
        _renamingValidatorServiceMock.Verify(x => x.ValidateMedia(It.IsAny<Media>()), Times.Exactly(2));
        
        _processorChainFactoryMock.Verify(x => x.Build(MediaType.Image), Times.Exactly(2));
        
        imageProcessor.Verify(x => x.Process(It.Is<ProcessMediaRequest>(r =>
            r.Original == imageMedia1 &&
            r.Target == imageMedia1)), Times.Once);
        
        imageProcessor.Verify(x => x.Process(It.Is<ProcessMediaRequest>(r =>
            r.Original == imageMedia2 &&
            r.Target == imageMedia2)), Times.Once);
    }

    [Test]
    public async Task RunAsync_SkipsMedia_WhenIsNotValid()
    {
        var imageFile = CreateFileInfo("/test/IMG_0001.jpg");

        _ioWrapperMock.Setup(x => x.GetAllFilesByExtensions("/test", It.Is<string[]>(e => e.SequenceEqual(new[] { ".jpg" }))))
            .Returns([imageFile]);

        var imageMedia = new Media
        {
            Path = imageFile.FullName, 
            CreatedDateTime = DateTimeOffset.UtcNow,
            Type = MediaType.Image
        };

        _mediaFactoryMock.Setup(x => x.CreateMedia(imageFile.FullName))
            .Returns(imageMedia);
        
        _renamingValidatorServiceMock.Setup(x => x.ValidateMedia(It.IsAny<Media>()))
            .Returns(false);

        var sut = new RenamingOrchestrator(
            _loggerMock.Object,
            _mediaExtensionsSettingsOptions,
            _renamingSettingsOptions,
            _ioWrapperMock.Object,
            _mediaFactoryMock.Object,
            _renamingServiceMock.Object,
            _processorChainFactoryMock.Object,
            _renamingValidatorServiceMock.Object);
        
        await sut.RunAsync(CancellationToken.None);
        
        _renamingValidatorServiceMock.Verify(x => x.ValidateMedia(It.IsAny<Media>()), Times.Once);
        
        _renamingServiceMock.Verify(x => x.Rename(It.IsAny<Media>()), Times.Never);
        _processorChainFactoryMock.Verify(x => x.Build(It.IsAny<MediaType>()), Times.Never);
    }

    private static FileInfo CreateFileInfo(string fullName)
    {
        var fileInfo = new FileInfo(fullName);
        return fileInfo;
    }
}