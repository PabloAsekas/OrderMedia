using OrderMedia.Enums;
using OrderMedia.Interfaces.Factories;
using OrderMedia.Interfaces.Handlers;
using OrderMedia.Models;
using OrderMedia.Services;

namespace OrderMediaTests.Services
{
    public class ClassificationServiceTests
	{
		private AutoMocker _autoMocker;
		private Mock<IProcessorHandler> _processorHandlerMock;
		private Mock<IProcessorHandlerFactory> _processorHandlerFactoryMock;

		[SetUp]
		public void SetUp()
		{
			_autoMocker = new AutoMocker();

			_processorHandlerMock = _autoMocker.GetMock<IProcessorHandler>();

			_processorHandlerFactoryMock = _autoMocker.GetMock<IProcessorHandlerFactory>();
			_processorHandlerFactoryMock.Setup(x => x.CreateProcessorHandler(It.IsAny<MediaType>()))
				.Returns(_processorHandlerMock.Object);
		}

		[Test]
		public void Process_Runs_Successfully()
		{
			// Arrange
			var media = new Media()
			{
				MediaType = MediaType.Image,
			};

			var sut = _autoMocker.CreateInstance<ClassificationService>();

			// Act
			sut.Process(media);

			// Assert
			_processorHandlerFactoryMock.Verify(x => x.CreateProcessorHandler(media.MediaType), Times.Once);
			_processorHandlerMock.Verify(x => x.Process(media), Times.Once);
		}
	}
}