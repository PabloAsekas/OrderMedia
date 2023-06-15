using OrderMedia.Enums;
using OrderMedia.Interfaces;
using OrderMedia.Models;
using OrderMedia.Services;

namespace OrderMediaTests.Services
{
    public class ClassificationServiceTests
	{
		private AutoMocker _autoMocker;
		private Mock<IProcessor> _processorMock;
		private Mock<IProcessorFactory> _processorFactoryMock;

		[SetUp]
		public void SetUp()
		{
			_autoMocker = new AutoMocker();

			_processorMock = _autoMocker.GetMock<IProcessor>();

			_processorFactoryMock = _autoMocker.GetMock<IProcessorFactory>();
			_processorFactoryMock.Setup(x => x.CreateProcessor(It.IsAny<MediaType>()))
				.Returns(_processorMock.Object);
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
			_processorFactoryMock.Verify(x => x.CreateProcessor(media.MediaType), Times.Once);
			_processorMock.Verify(x => x.Execute(media), Times.Once);
		}
	}
}