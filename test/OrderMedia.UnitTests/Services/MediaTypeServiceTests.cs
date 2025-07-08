using OrderMedia.Enums;
using OrderMedia.Interfaces;
using OrderMedia.Services;

namespace OrderMedia.UnitTests.Services;

[TestFixture]
public class MediaTypeServiceTests
{
	private Mock<IIoWrapper> _ioWrapperMock;

	[SetUp]
	public void SetUp()
	{
		_ioWrapperMock = new Mock<IIoWrapper>();
	}
	
	[TestCase("test", ".gif", MediaType.Image)]
    [TestCase("test", ".heic", MediaType.Image)]
    [TestCase("test", ".jpeg", MediaType.Image)]
    [TestCase("test", ".jpg", MediaType.Image)]
    [TestCase("test", ".png", MediaType.Image)]
    [TestCase("test", ".mov", MediaType.Video)]
    [TestCase("test", ".mp4", MediaType.Video)]
    [TestCase("PHOTO_WHATSAPP", ".jpg", MediaType.WhatsAppImage)]
    [TestCase("VIDEO_WHATSAPP", ".mov", MediaType.WhatsAppVideo)]
    [TestCase("GIF_WHATSAPP", ".gif", MediaType.WhatsAppImage)]
    [TestCase("test", ".arw", MediaType.Raw)]
    [TestCase("test", ".dng", MediaType.Raw)]
    [TestCase("test", ".insp", MediaType.Image)]
    [TestCase("test", ".insv", MediaType.Insv)]
    public void GetMediaType_Returns_MediaType_Successfully(string name, string extension, MediaType mediaType)
	{
		// Arrange
		var path = name + extension;

		_ioWrapperMock.Setup(x => x.GetExtension(path))
			.Returns(extension);
		_ioWrapperMock.Setup(x => x.GetFileNameWithoutExtension(path))
			.Returns(name);

		var sut = new MediaTypeService(_ioWrapperMock.Object);

		// Act
		var result = sut.GetMediaType(path);

		// Assert
		result.Should().Be(mediaType);
	}

    [Test]
    public void GetMediaType_Throws_Exception()
    {
        // Arrange
        const string name = "test";
        const string extension = ".test";
        const string path = $"{name}{extension}";

        _ioWrapperMock.Setup(x => x.GetExtension(path))
            .Returns(extension);
        _ioWrapperMock.Setup(x => x.GetFileNameWithoutExtension(path))
            .Returns(name);

        var sut = new MediaTypeService(_ioWrapperMock.Object);

        // Act
        Action act = () => sut.GetMediaType(path);

        // Assert
        act.Should().Throw<FormatException>();
    }
}
