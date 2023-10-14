using OrderMedia.Enums;
using OrderMedia.Factories;
using OrderMedia.Interfaces;
using OrderMedia.Services.Processors;

namespace OrderMediaTests.Factories
{
    public class ProcessorFactoryTests
	{
        private AutoMocker _autoMocker;
		private Mock<IServiceProvider> _serviceProviderMock;

		[SetUp]
		public void SetUp()
		{
			_autoMocker = new AutoMocker();

			var ioServiceMock = _autoMocker.GetMock<IIOService>();
			var renameServiceMock = _autoMocker.GetMock<IRenameService>();

            var mainProcessor = new MainProcessor(ioServiceMock.Object);

			var livePhotoProcessor = new LivePhotoProcessor(ioServiceMock.Object);

			var aaeProcessor = new AaeProcessor(ioServiceMock.Object, renameServiceMock.Object);

            var xmpProcessor = new XmpProcessor(ioServiceMock.Object, renameServiceMock.Object);

            _serviceProviderMock = _autoMocker.GetMock<IServiceProvider>();
			_serviceProviderMock.Setup(x => x.GetService(typeof(MainProcessor)))
				.Returns(mainProcessor);
            _serviceProviderMock.Setup(x => x.GetService(typeof(LivePhotoProcessor)))
                .Returns(livePhotoProcessor);
            _serviceProviderMock.Setup(x => x.GetService(typeof(AaeProcessor)))
                .Returns(aaeProcessor);
            _serviceProviderMock.Setup(x => x.GetService(typeof(XmpProcessor)))
                .Returns(xmpProcessor);
        }

		[Test]
		[TestCase(MediaType.Image, typeof(MainProcessor))]
        [TestCase(MediaType.Raw, typeof(MainProcessor))]
        [TestCase(MediaType.Video, typeof(MainProcessor))]
        [TestCase(MediaType.WhatsAppImage, typeof(MainProcessor))]
        [TestCase(MediaType.WhatsAppVideo, typeof(MainProcessor))]
        public void CreateProcessor_Returns_Succesfully(MediaType mediaType, Type processor)
		{
			// Arrange
			var sut = _autoMocker.CreateInstance<ProcessorFactory>();

			// Act
			var result = sut.CreateProcessor(mediaType);

			// Assert
			result.Should().BeOfType(processor);
        }

        [Test]
		public void CreateProcessor_Throws_Exception()
		{
			// Arrange
			var sut = _autoMocker.CreateInstance<ProcessorFactory>();

			// Act
			Action act = () => sut.CreateProcessor(MediaType.None);

			// Assert
			act.Should().Throw<FormatException>();
		}
	}
}

