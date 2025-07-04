﻿using OrderMedia.Enums;
using OrderMedia.Factories;
using OrderMedia.Interfaces;
using OrderMedia.Interfaces.Factories;
using OrderMedia.Models;

namespace OrderMedia.UnitTests.Factories;

[TestFixture]
public class MediaFactoryTests
{
    private AutoMocker _autoMocker;

    private Mock<IConfigurationService> _configurationServiceMock;
    private Mock<IIOService> _ioServiceMock;
    private Mock<IRenameStrategyFactory> _renameStrategyFactoryMock;
    private Mock<IMediaTypeService> _mediaTypeServiceMock;
    private Mock<ICreatedDateExtractorService> _createdDateExtractorServiceMock;

    private const string mediaPath = $"{mediaFolder}/{name}";
    private const string mediaFolder = "test/path";
    private const string name = "test.jpg";
    private const string nameWithoutExtension = "test";
    private static DateTimeOffset createdDateTimeOffset = new DateTimeOffset(new DateTime(2014, 7, 31, 22, 15, 15));
    private const string createdDateTimeOffsetFolder = "2014-07-31";
    private const string classificationFolderName = "img";
    private const string newMediaFolder = $"{mediaFolder}/{classificationFolderName}/{createdDateTimeOffsetFolder}/";
    private const string newName = "renamed.jpg";
    private const string newNameWithoutExtension = "renamed";

    [SetUp]
    public void SetUp()
    {
        _autoMocker = new AutoMocker();

        _configurationServiceMock = _autoMocker.GetMock<IConfigurationService>();
        _configurationServiceMock.Setup(x => x.GetImageFolderName())
            .Returns(classificationFolderName);
        _configurationServiceMock.Setup(x => x.GetVideoFolderName())
            .Returns(classificationFolderName);

        _ioServiceMock = _autoMocker.GetMock<IIOService>();
        _ioServiceMock.Setup(x => x.GetDirectoryName(It.IsAny<string>()))
            .Returns(mediaFolder);
        _ioServiceMock.Setup(x => x.GetFileName(mediaPath))
            .Returns(name);
        _ioServiceMock.Setup(x => x.GetFileNameWithoutExtension(mediaPath))
            .Returns(nameWithoutExtension);
        _ioServiceMock.Setup(x => x.Combine(new string[] { mediaFolder, classificationFolderName, createdDateTimeOffset.ToString("yyyy-MM-dd") }))
            .Returns(newMediaFolder);
        _ioServiceMock.Setup(x => x.GetFileNameWithoutExtension(newName))
            .Returns(newNameWithoutExtension);
        _ioServiceMock.Setup(x => x.GetFileNameWithoutExtension(name))
            .Returns(nameWithoutExtension);

        var renameServiceMock = _autoMocker.GetMock<IRenameStrategy>();
        renameServiceMock.Setup(x => x.Rename(It.IsAny<string>(), It.IsAny<DateTimeOffset>()))
            .Returns(newName);

        _mediaTypeServiceMock = _autoMocker.GetMock<IMediaTypeService>();

        _createdDateExtractorServiceMock = _autoMocker.GetMock<ICreatedDateExtractorService>();
        _createdDateExtractorServiceMock.Setup(x => x.GetCreatedDateTimeOffset(It.IsAny<string>()))
            .Returns(createdDateTimeOffset);

        _renameStrategyFactoryMock = _autoMocker.GetMock<IRenameStrategyFactory>();
        _renameStrategyFactoryMock.Setup(x => x.GetRenameStrategy(It.IsAny<MediaType>()))
            .Returns(renameServiceMock.Object);
    }
    
    [TestCase(MediaType.Image, true)]
    [TestCase(MediaType.Image, false)]
    [TestCase(MediaType.Raw, true)]
    [TestCase(MediaType.Raw, false)]
    [TestCase(MediaType.Video, true)]
    [TestCase(MediaType.Video, false)]
    [TestCase(MediaType.WhatsAppImage, true)]
    [TestCase(MediaType.WhatsAppImage, false)]
    [TestCase(MediaType.WhatsAppVideo, true)]
    [TestCase(MediaType.WhatsAppVideo, false)]
    [TestCase(MediaType.Insv, true)]
    [TestCase(MediaType.Insv, false)]
    public void CreateMedia_Returns_Successfully(MediaType mediaType, bool renamed)
    {
        // Arrange
        var finalNewName = renamed ? newName : name;

        var finalNewNameWithoutExtension = renamed ? newNameWithoutExtension : nameWithoutExtension;

        var finalMediaPath = $"{newMediaFolder}/{finalNewName}";

        _ioServiceMock.Setup(x => x.Combine(new string[] { newMediaFolder, finalNewName }))
            .Returns(finalMediaPath);

        _mediaTypeServiceMock.Setup(x => x.GetMediaType(It.IsAny<string>()))
            .Returns(mediaType);

        _configurationServiceMock.Setup(x => x.GetRenameMediaFiles())
            .Returns(renamed);

        var sut = _autoMocker.CreateInstance<MediaFactory>();

        // Act
        var result = sut.CreateMedia(mediaPath);

        // Assert
        result.Should().BeOfType<Media>();
        result.MediaType.Should().Be(mediaType);
        result.MediaPath.Should().Be(mediaPath);
        result.MediaFolder.Should().Be(mediaFolder);
        result.Name.Should().Be(name);
        result.NameWithoutExtension.Should().Be(nameWithoutExtension);
        result.CreatedDateTimeOffset.Should().Be(createdDateTimeOffset);
        result.NewMediaPath.Should().Be(finalMediaPath);
        result.NewMediaFolder.Should().Be(newMediaFolder);
        result.NewName.Should().Be(finalNewName);
        result.NewNameWithoutExtension.Should().Be(finalNewNameWithoutExtension);
    }

    [Test]
    public void CreateMedia_Throws_Exception()
    {
        // Arrange
        _mediaTypeServiceMock.Setup(x => x.GetMediaType(mediaPath))
            .Returns(MediaType.None);

        var sut = _autoMocker.CreateInstance<MediaFactory>();

        // Act
        Action act = () => sut.CreateMedia(mediaPath);

        // Assert
        act.Should().Throw<FormatException>();
    }
}