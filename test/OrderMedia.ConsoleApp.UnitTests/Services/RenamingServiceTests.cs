using FluentAssertions;
using Microsoft.Extensions.Options;
using Moq;
using OrderMedia.ConsoleApp.Configuration;
using OrderMedia.ConsoleApp.Services;
using OrderMedia.Enums;
using OrderMedia.Interfaces;
using OrderMedia.Interfaces.Factories;
using OrderMedia.Models;

namespace OrderMedia.ConsoleApp.UnitTests.Services;

[TestFixture]
public class RenamingServiceTests
{
    private Mock<IRenameStrategyFactory> _renameStrategyFactoryMock;
    private IOptions<RenamingSettings> _renamingSettingsOptions;
    private Mock<IIoWrapper> _ioWrapperMock;

    [SetUp]
    public void Setup()
    {
        _renameStrategyFactoryMock = new Mock<IRenameStrategyFactory>();
        _renamingSettingsOptions = Options.Create(new RenamingSettings()
        {
            ReplaceLongNames = true,
            MaxMediaNameLength = 8,
            NewMediaName = "test"
        });
        _ioWrapperMock = new Mock<IIoWrapper>();
    }

    [Test]
    public void Rename_RunSuccessfully_WhenMediaIsValid()
    {
        // Arrange
        const string mediaExtension = "jpg";
        const string photoName = "test";
        const string mediaName = $"{photoName}.{mediaExtension}";
        var mediaCreatedDate = new DateTime(2024, 07, 31, 12, 0, 0);
        const MediaType mediaType = MediaType.Image;
        const string directoryPath = "/test/photos/";
        var newPhotoName = $"{mediaCreatedDate:yyyy-MM-dd_HH-mm-ss}_{photoName}";
        var renamedName = $"{mediaCreatedDate:yyyy-MM-dd_HH-mm-ss}_{newPhotoName}.{mediaExtension}";
        
        var media = new Media
        {
            Name = mediaName,
            CreatedDateTime = mediaCreatedDate,
            Type = mediaType,
            DirectoryPath = directoryPath
        };
        
        var renameStrategyMock = new Mock<IRenameStrategy>();

        renameStrategyMock.Setup(x => x.Rename(It.IsAny<RenameMediaRequest>()))
            .Returns(renamedName);
        
        _renameStrategyFactoryMock.Setup(x => x.GetRenameStrategy(It.IsAny<MediaType>()))
            .Returns(renameStrategyMock.Object);
        
        _ioWrapperMock.Setup(x => x.Combine(new[]{media.DirectoryPath, renamedName}))
            .Returns($"{media.DirectoryPath}{renamedName}");

        _ioWrapperMock.Setup(x => x.GetFileNameWithoutExtension(renamedName))
            .Returns(newPhotoName);

        var sut = new RenamingService(
            _renameStrategyFactoryMock.Object,
            _renamingSettingsOptions,
            _ioWrapperMock.Object);

        // Act
        var result = sut.Rename(media);
        
        // Assert
        result.Type.Should().Be(mediaType);
        result.Path.Should().Be($"{media.DirectoryPath}{renamedName}");
        result.DirectoryPath.Should().Be(directoryPath);
        result.Name.Should().Be(renamedName);
        result.NameWithoutExtension.Should().Be(newPhotoName);
        result.CreatedDateTime.Should().Be(mediaCreatedDate);
    }
}