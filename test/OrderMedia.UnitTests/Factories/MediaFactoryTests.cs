using Microsoft.Extensions.Options;
using OrderMedia.Enums;
using OrderMedia.Factories;
using OrderMedia.Interfaces;
using OrderMedia.Interfaces.Factories;
using OrderMedia.Models;
using OrderMedia.Configuration;

namespace OrderMedia.UnitTests.Factories;

[TestFixture]
public class MediaFactoryTests
{
    private Mock<IIoWrapper> _ioWrapperMock;
    private Mock<IRenameStrategyFactory> _renameStrategyFactoryMock;
    private Mock<IMediaTypeService> _mediaTypeServiceMock;
    private Mock<ICreatedDateExtractorService> _createdDateExtractorServiceMock;

    private const string MediaPath = $"{MediaFolder}/{Name}";
    private const string MediaFolder = "test/path";
    private const string Name = "test.jpg";
    private const string NameWithoutExtension = "test";
    private static readonly DateTimeOffset CreatedDateTimeOffset = new(new DateTime(2014, 7, 31, 22, 15, 15));
    private const string CreatedDateTimeOffsetFolder = "2014-07-31";
    private const string ClassificationFolderName = "img";
    private const string NewMediaFolder = $"{MediaFolder}/{ClassificationFolderName}/{CreatedDateTimeOffsetFolder}/";
    private const string NewName = "renamed.jpg";
    private const string NewNameWithoutExtension = "renamed";

    [SetUp]
    public void SetUp()
    {
        _ioWrapperMock = new Mock<IIoWrapper>();
        _ioWrapperMock.Setup(x => x.GetDirectoryName(It.IsAny<string>()))
            .Returns(MediaFolder);
        _ioWrapperMock.Setup(x => x.GetFileName(MediaPath))
            .Returns(Name);
        _ioWrapperMock.Setup(x => x.GetFileNameWithoutExtension(MediaPath))
            .Returns(NameWithoutExtension);
        _ioWrapperMock.Setup(x => x.Combine(new string[] { MediaFolder, ClassificationFolderName, CreatedDateTimeOffset.ToString("yyyy-MM-dd") }))
            .Returns(NewMediaFolder);
        _ioWrapperMock.Setup(x => x.GetFileNameWithoutExtension(NewName))
            .Returns(NewNameWithoutExtension);
        _ioWrapperMock.Setup(x => x.GetFileNameWithoutExtension(Name))
            .Returns(NameWithoutExtension);

        var renameServiceMock = new Mock<IRenameStrategy>();
        renameServiceMock.Setup(x => x.Rename(It.IsAny<string>(), It.IsAny<DateTimeOffset>()))
            .Returns(NewName);

        _mediaTypeServiceMock = new Mock<IMediaTypeService>();

        _createdDateExtractorServiceMock = new Mock<ICreatedDateExtractorService>();
        _createdDateExtractorServiceMock.Setup(x => x.GetCreatedDateTimeOffset(It.IsAny<string>()))
            .Returns(CreatedDateTimeOffset);

        _renameStrategyFactoryMock = new Mock<IRenameStrategyFactory>();
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
        var finalNewName = renamed ? NewName : Name;

        var finalNewNameWithoutExtension = renamed ? NewNameWithoutExtension : NameWithoutExtension;

        var finalMediaPath = $"{NewMediaFolder}/{finalNewName}";

        _ioWrapperMock.Setup(x => x.Combine(new[] { NewMediaFolder, finalNewName }))
            .Returns(finalMediaPath);

        _mediaTypeServiceMock.Setup(x => x.GetMediaType(It.IsAny<string>()))
            .Returns(mediaType);

        var classificationSettingsOptions = Options.Create(new ClassificationSettings
        {
            Folders = new ClassificationFolders()
            {
                ImageFolderName = ClassificationFolderName,
                VideoFolderName = ClassificationFolderName,
            },
            RenameMediaFiles = renamed,
        });
        
        var sut = new MediaFactory(
            _ioWrapperMock.Object,
            _renameStrategyFactoryMock.Object,
            _mediaTypeServiceMock.Object,
            _createdDateExtractorServiceMock.Object,
            classificationSettingsOptions
            );

        // Act
        var result = sut.CreateMedia(MediaPath);

        // Assert
        result.Should().BeOfType<Media>();
        result.Type.Should().Be(mediaType);
        result.Path.Should().Be(MediaPath);
        result.DirectoryPath.Should().Be(MediaFolder);
        result.Name.Should().Be(Name);
        result.NameWithoutExtension.Should().Be(NameWithoutExtension);
        result.CreatedDateTime.Should().Be(CreatedDateTimeOffset);
        result.NewMediaPath.Should().Be(finalMediaPath);
        result.NewMediaFolder.Should().Be(NewMediaFolder);
        result.NewName.Should().Be(finalNewName);
        result.NewNameWithoutExtension.Should().Be(finalNewNameWithoutExtension);
    }

    [Test]
    public void CreateMedia_Throws_Exception()
    {
        // Arrange
        _mediaTypeServiceMock.Setup(x => x.GetMediaType(MediaPath))
            .Returns(MediaType.None);
        
        var classificationSettingsOptions = Options.Create(new ClassificationSettings
        {
            Folders = new ClassificationFolders()
            {
                ImageFolderName = ClassificationFolderName,
                VideoFolderName = ClassificationFolderName,
            },
        });

        var sut = new MediaFactory(
            _ioWrapperMock.Object,
            _renameStrategyFactoryMock.Object,
            _mediaTypeServiceMock.Object,
            _createdDateExtractorServiceMock.Object,
            classificationSettingsOptions
            );

        // Act
        Action act = () => sut.CreateMedia(MediaPath);

        // Assert
        act.Should().Throw<FormatException>();
    }
}