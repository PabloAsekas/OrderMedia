using OrderMedia.Enums;
using OrderMedia.Interfaces.Factories;
using OrderMedia.Interfaces.Handlers;
using OrderMedia.Models;
using OrderMedia.Services;

namespace OrderMedia.UnitTests.Services;

[TestFixture]
public class ClassificationServiceTests
{
	private Mock<IProcessorChainFactory> _processorChainFactoryMock;
	private Mock<IProcessorHandler> _processorHandlerMock;

	[SetUp]
	public void SetUp()
	{
		_processorHandlerMock = new Mock<IProcessorHandler>();
		
		_processorChainFactoryMock = new Mock<IProcessorChainFactory>();
		_processorChainFactoryMock.Setup(x => x.Build(It.IsAny<MediaType>()))
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

		var sut = new ClassificationService(_processorChainFactoryMock.Object);

		// Act
		sut.Process(media);

		// Assert
		_processorChainFactoryMock.Verify(x => x.Build(media.MediaType), Times.Once);
		_processorHandlerMock.Verify(x => x.Process(media), Times.Once);
	}
}
