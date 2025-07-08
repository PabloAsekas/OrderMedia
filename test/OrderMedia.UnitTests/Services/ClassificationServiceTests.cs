using OrderMedia.Enums;
using OrderMedia.Interfaces.Factories;
using OrderMedia.Interfaces.Handlers;
using OrderMedia.Models;
using OrderMedia.Services;

namespace OrderMedia.UnitTests.Services;

[TestFixture]
public class ClassificationServiceTests
{
	private Mock<IProcessorHandler> _processorHandlerMock;
	private Mock<IProcessorHandlerFactory> _processorHandlerFactoryMock;

	[SetUp]
	public void SetUp()
	{
		_processorHandlerMock = new Mock<IProcessorHandler>();

		_processorHandlerFactoryMock = new Mock<IProcessorHandlerFactory>();
		_processorHandlerFactoryMock.Setup(x => x.CreateProcessorHandler(It.IsAny<MediaType>()))
			.Returns(_processorHandlerMock.Object);
	}

	[Test]
	public void Process_Runs_Successfully()
	{
		// Arrange
		var media = new Media
		{
			MediaType = MediaType.Image,
		};

		var sut = new ClassificationService(_processorHandlerFactoryMock.Object);

		// Act
		sut.Process(media);

		// Assert
		_processorHandlerFactoryMock.Verify(x => x.CreateProcessorHandler(media.MediaType), Times.Once);
		_processorHandlerMock.Verify(x => x.Process(media), Times.Once);
	}
}
