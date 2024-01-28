using OrderMedia.Enums;
using OrderMedia.Factories;
using OrderMedia.Interfaces;
using OrderMedia.Services.CreatedDateExtractors;

namespace OrderMediaTests.Factories
{
    public class CreatedDateExtractorsFactoryTests
	{
		private AutoMocker _autoMocker;
		private Mock<IServiceProvider> _serviceProviderMock;

		[SetUp]
		public void SetUp()
		{
			_autoMocker = new AutoMocker();

			var metadataExtractorServiceMock = _autoMocker.GetMock<IMetadataExtractorService>();

			var ioServiceMock = _autoMocker.GetMock<IIOService>();
			var xmpExtractorServiceMock = _autoMocker.GetMock<IXmpExtractorService>();

			_serviceProviderMock = _autoMocker.GetMock<IServiceProvider>();
			_serviceProviderMock.Setup(x => x.GetService(typeof(ImageCreatedDateExtractor)))
				.Returns(new ImageCreatedDateExtractor(metadataExtractorServiceMock.Object));
			_serviceProviderMock.Setup(x => x.GetService(typeof(RawCreatedDateExtractor)))
				.Returns(new RawCreatedDateExtractor(metadataExtractorServiceMock.Object, ioServiceMock.Object, xmpExtractorServiceMock.Object));
			_serviceProviderMock.Setup(x => x.GetService(typeof(VideoCreatedDateExtractor)))
				.Returns(new VideoCreatedDateExtractor(metadataExtractorServiceMock.Object));
			_serviceProviderMock.Setup(x => x.GetService(typeof(WhatsAppCreatedDateExtractor)))
				.Returns(new WhatsAppCreatedDateExtractor(ioServiceMock.Object));
		}

		[Test]
		[TestCase(MediaType.Image, typeof(ImageCreatedDateExtractor))]
		[TestCase(MediaType.Raw, typeof(RawCreatedDateExtractor))]
		[TestCase(MediaType.Video, typeof(VideoCreatedDateExtractor))]
		[TestCase(MediaType.WhatsAppImage, typeof(WhatsAppCreatedDateExtractor))]
		[TestCase(MediaType.WhatsAppVideo, typeof(WhatsAppCreatedDateExtractor))]
        public void GetExtractor_Returns_Successfully(MediaType mediaType, Type createdDataExtractors)
		{
			// Arrange
			var sut = _autoMocker.CreateInstance<CreatedDateExtractorsFactory>();

			// Act
			var result = sut.GetExtractor(mediaType);

			// Assert
			result.Should().BeOfType(createdDataExtractors);
			_serviceProviderMock.Verify(x => x.GetService(createdDataExtractors), Times.Once);
		}

		[Test]
		public void GetExtractor_Throws_Exception()
		{
            // Arrange
            var sut = _autoMocker.CreateInstance<CreatedDateExtractorsFactory>();

            // Act
            Action act = () => sut.GetExtractor(MediaType.None);

            // Assert
            act.Should().Throw<FormatException>();
            _serviceProviderMock.Verify(x => x.GetService(It.IsAny<Type>()), Times.Never);
        }
	}
}

