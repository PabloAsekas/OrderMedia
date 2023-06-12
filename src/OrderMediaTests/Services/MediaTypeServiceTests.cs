using System;
using FluentAssertions;
using Moq;
using Moq.AutoMock;
using OrderMedia.Enums;
using OrderMedia.Interfaces;
using OrderMedia.Services;

namespace OrderMediaTests.Services
{
	public class MediaTypeServiceTests
	{
		private AutoMocker _autoMocker;
		private Mock<IIOService> _ioServiceMock;

		[SetUp]
		public void SetUp()
		{
			_autoMocker = new AutoMocker();

			_ioServiceMock = _autoMocker.GetMock<IIOService>();
		}

		[Test]
		[TestCase("test", ".gif", MediaType.Image)]
        [TestCase("test", ".heic", MediaType.Image)]
        [TestCase("test", ".jpeg", MediaType.Image)]
        [TestCase("test", ".jpg", MediaType.Image)]
        [TestCase("test", ".png", MediaType.Image)]
        [TestCase("test", ".mov", MediaType.Video)]
        [TestCase("test", ".mp4", MediaType.Video)]
        [TestCase("PHOTO_WHATSAPP", ".jpg", MediaType.WhatsApp)]
        [TestCase("VIDEO_WHATSAPP", ".mov", MediaType.WhatsApp)]
        [TestCase("test", ".arw", MediaType.Raw)]
        [TestCase("test", ".dng", MediaType.Raw)]
        public void GetMediaType_Returns_MediaType_Successfully(string name, string extension, MediaType mediaType)
		{
			// Arrange
			var path = name + extension;

			_ioServiceMock.Setup(x => x.GetExtension(path))
				.Returns(extension);
			_ioServiceMock.Setup(x => x.GetFileNameWithoutExtension(path))
				.Returns(name);

			var sut = _autoMocker.CreateInstance<MediaTypeService>();

			// Act
			var result = sut.GetMediaType(path);

			// Assert
			result.Should().Be(mediaType);
		}

        [Test]
        public void GetMediaType_Throws_Exception()
        {
            // Arrange
            var name = "test";
            var extension = ".test";
            var path = $"{name}{extension}";

            _ioServiceMock.Setup(x => x.GetExtension(path))
                .Returns(extension);
            _ioServiceMock.Setup(x => x.GetFileNameWithoutExtension(path))
                .Returns(name);

            var sut = _autoMocker.CreateInstance<MediaTypeService>();

            // Act
            Action act = () => sut.GetMediaType(path);

            // Assert
            act.Should().Throw<FormatException>();
        }
    }
}

