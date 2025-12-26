using FluentAssertions;
using Microsoft.Extensions.Options;
using Moq;
using OrderMedia.ConsoleApp.Configuration;
using OrderMedia.ConsoleApp.Interfaces;
using OrderMedia.ConsoleApp.Services;
using OrderMedia.Enums;
using OrderMedia.Interfaces;
using OrderMedia.Interfaces.Factories;
using OrderMedia.Models;

namespace OrderMedia.ConsoleApp.UnitTests.Services;

[TestFixture]
public class ClassificationServiceTests
{
    Mock<IIoWrapper> _mockIoWrapper;
    Mock<IRenameStrategyFactory> _mockRenameStrategyFactory;
    Mock<IClassificationMediaFolderStrategyResolver> _mockClassificationMediaFolderStrategyResolver;

    [SetUp]
    public void SetUp()
    {
        _mockIoWrapper = new Mock<IIoWrapper>();
        _mockRenameStrategyFactory = new Mock<IRenameStrategyFactory>();
        _mockClassificationMediaFolderStrategyResolver = new Mock<IClassificationMediaFolderStrategyResolver>();
    }

    [Test]
    public void Classify_ReturnsTargetMediaWithOriginalName_WhenRenameMediaFilesIsFalse()
    {
        // Arrange
        const string originalNameWithoutExtension = "image";
        const string originalExtension = ".jpg";
        const string originalName = $"{originalNameWithoutExtension}{originalExtension}";
        const string originalDirectoryPath = "/some/path/";
        const string originalPath = $"{originalDirectoryPath}{originalName}";
        const string classificationFolder = "img";
        const string date ="2014-07-31";
        const string targetDirectoryPath = $"{originalDirectoryPath}{classificationFolder}/{date}/";
        const string targetPath = $"{targetDirectoryPath}{originalName}";
        DateTimeOffset originalCreatedDateTime = DateTime.Parse(date);
        
        var original = new Media
        {
            Type = MediaType.Image,
            Path = originalPath,
            DirectoryPath = originalDirectoryPath,
            Name = originalName,
            NameWithoutExtension =  originalNameWithoutExtension,
            CreatedDateTime = originalCreatedDateTime
        };
        
        Mock<IClassificationMediaFolderStrategy> mockClassificationMediaFolderStrategy = new Mock<IClassificationMediaFolderStrategy>();
        mockClassificationMediaFolderStrategy.Setup(x => x.GetTargetFolder())
            .Returns(classificationFolder);
        _mockClassificationMediaFolderStrategyResolver.Setup(x => x.Resolve(It.IsAny<MediaType>()))
            .Returns(mockClassificationMediaFolderStrategy.Object);
        
        _mockIoWrapper.Setup(x => x.Combine(new []{originalDirectoryPath, classificationFolder, date}))
            .Returns(targetDirectoryPath);
        
        var settings = Options.Create(new ClassificationSettings
        {
            RenameMediaFiles = false,
        });
        
        _mockIoWrapper.Setup(x => x.Combine(new []{targetDirectoryPath, originalName}))
            .Returns(targetPath);
        _mockIoWrapper.Setup(x => x.GetFileNameWithoutExtension(originalName))
            .Returns(originalNameWithoutExtension);
        
        
        var sut = new ClassificationService(_mockIoWrapper.Object, settings, _mockRenameStrategyFactory.Object, _mockClassificationMediaFolderStrategyResolver.Object);
        
        // Act
        var result = sut.Classify(original);
        
        // Assert
        result.Should().BeOfType<Media>();
        result.Path.Should().Be(targetPath);
        result.DirectoryPath.Should().Be(targetDirectoryPath);
        result.Type.Should().Be(MediaType.Image);
        result.Name.Should().Be(originalName);
        result.NameWithoutExtension.Should().Be(originalNameWithoutExtension);
        result.CreatedDateTime.Should().Be(originalCreatedDateTime);
        _mockRenameStrategyFactory.Verify(x => x.GetRenameStrategy(It.IsAny<MediaType>()), Times.Never);
    }
    
    [Test]
    public void Classify_ReturnsTargetMediaWithModifiedName_WhenRenameMediaFilesIsTrue()
    {
        // Arrange
        const string originalNameWithoutExtension = "image";
        const string originalExtension = ".jpg";
        const string originalName = $"{originalNameWithoutExtension}{originalExtension}";
        const string originalDirectoryPath = "/some/path/";
        const string originalPath = $"{originalDirectoryPath}{originalName}";
        const string classificationFolder = "img";
        const string date ="2014-07-31";
        const string targetNameWithoutExtension = "modified";
        const string targetName = $"{targetNameWithoutExtension}{originalExtension}";
        const string targetDirectoryPath = $"{originalDirectoryPath}{classificationFolder}/{date}/";
        const string targetPath = $"{targetDirectoryPath}{targetName}";
        DateTimeOffset originalCreatedDateTime = DateTime.Parse(date);
        
        var original = new Media
        {
            Type = MediaType.Image,
            Path = originalPath,
            DirectoryPath = originalDirectoryPath,
            Name = originalName,
            NameWithoutExtension =  originalNameWithoutExtension,
            CreatedDateTime = originalCreatedDateTime
        };
        
        Mock<IClassificationMediaFolderStrategy> mockClassificationMediaFolderStrategy = new Mock<IClassificationMediaFolderStrategy>();
        mockClassificationMediaFolderStrategy.Setup(x => x.GetTargetFolder())
            .Returns(classificationFolder);
        _mockClassificationMediaFolderStrategyResolver.Setup(x => x.Resolve(It.IsAny<MediaType>()))
            .Returns(mockClassificationMediaFolderStrategy.Object);
        
        _mockIoWrapper.Setup(x => x.Combine(new []{originalDirectoryPath, classificationFolder, date}))
            .Returns(targetDirectoryPath);
        
        var settings = Options.Create(new ClassificationSettings
        {
            RenameMediaFiles = true
        });
        var strategy = new Mock<IRenameStrategy>();
        strategy.Setup(x => x.Rename(It.IsAny<RenameMediaRequest>()))
            .Returns(targetName);
        _mockRenameStrategyFactory.Setup(x => x.GetRenameStrategy(It.IsAny<MediaType>()))
            .Returns(strategy.Object);
        
        _mockIoWrapper.Setup(x => x.Combine(new []{targetDirectoryPath, targetName}))
            .Returns(targetPath);
        _mockIoWrapper.Setup(x => x.GetFileNameWithoutExtension(targetName))
            .Returns(targetNameWithoutExtension);
        
        var sut = new ClassificationService(_mockIoWrapper.Object, settings, _mockRenameStrategyFactory.Object, _mockClassificationMediaFolderStrategyResolver.Object);
        
        // Act
        var result = sut.Classify(original);
        
        // Assert
        result.Should().BeOfType<Media>();
        result.Path.Should().Be(targetPath);
        result.DirectoryPath.Should().Be(targetDirectoryPath);
        result.Type.Should().Be(MediaType.Image);
        result.Name.Should().Be(targetName);
        result.NameWithoutExtension.Should().Be(targetNameWithoutExtension);
        result.CreatedDateTime.Should().Be(originalCreatedDateTime);
        
        _mockRenameStrategyFactory.Verify(x => x.GetRenameStrategy(original.Type), Times.Once);
        strategy.Verify(x => x.Rename(It.IsAny<RenameMediaRequest>()), Times.Once);
    }
}