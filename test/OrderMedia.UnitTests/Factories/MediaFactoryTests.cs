using OrderMedia.Enums;
using OrderMedia.Factories;
using OrderMedia.Interfaces;
using OrderMedia.Models;

namespace OrderMedia.UnitTests.Factories;

[TestFixture]
public class MediaFactoryTests
{
    private Mock<IIoWrapper> _ioWrapperMock;
    private Mock<IMediaTypeService> _mediaTypeServiceMock;
    private Mock<ICreatedDateExtractorService> _createdDateExtractorServiceMock;

    private const string MediaPath = $"{MediaFolder}/{Name}";
    private const string MediaFolder = "test/path";
    private const string Name = "test.jpg";
    private const string NameWithoutExtension = "test";
    private static readonly DateTimeOffset CreatedDateTimeOffset = new(new DateTime(2014, 7, 31, 22, 15, 15));

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

        _mediaTypeServiceMock = new Mock<IMediaTypeService>();

        _createdDateExtractorServiceMock = new Mock<ICreatedDateExtractorService>();
        _createdDateExtractorServiceMock.Setup(x => x.GetCreatedDateTimeOffset(It.IsAny<string>()))
            .Returns(CreatedDateTimeOffset);
    }
    
    [TestCase(MediaType.Image)]
    [TestCase(MediaType.Raw)]
    [TestCase(MediaType.Video)]
    [TestCase(MediaType.WhatsAppImage)]
    [TestCase(MediaType.WhatsAppVideo)]
    [TestCase(MediaType.Insv)]
    public void CreateMedia_Returns_Successfully(MediaType mediaType)
    {
        // Arrange
        _mediaTypeServiceMock.Setup(x => x.GetMediaType(It.IsAny<string>()))
            .Returns(mediaType);
        
        var sut = new MediaFactory(
            _ioWrapperMock.Object,
            _mediaTypeServiceMock.Object,
            _createdDateExtractorServiceMock.Object
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
    }
}