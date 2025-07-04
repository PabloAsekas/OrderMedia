using OrderMedia.Enums;
using OrderMedia.Factories;
using OrderMedia.Handlers.Processor;
using OrderMedia.Interfaces;

namespace OrderMedia.UnitTests.Factories;

[TestFixture]
public class ProcessorHandlerFactoryTests
{
    private AutoMocker _autoMocker;
	private Mock<IServiceProvider> _serviceProviderMock;

	[SetUp]
	public void SetUp()
	{
		_autoMocker = new AutoMocker();

		var ioServiceMock = _autoMocker.GetMock<IIOService>();
		var aaeHelperServiceMock = _autoMocker.GetMock<IAaeHelperService>();
		var metadataAggregatorServiceMock = _autoMocker.GetMock<IMetadataAggregatorService>();
		
        var moveMediaProcessorHandler = new MoveMediaProcessorHandler(ioServiceMock.Object);

		var moveLivePhotoProcessorHandler = new MoveLivePhotoProcessorHandler(ioServiceMock.Object);

		var moveAaeProcessorHandler = new MoveAaeProcessorHandler(ioServiceMock.Object, aaeHelperServiceMock.Object);

        var moveXmpProcessorHandler = new MoveXmpProcessorHandler(ioServiceMock.Object);

        var createdDateAggregatorProcessor = new CreatedDateAggregatorProcessorHandler(metadataAggregatorServiceMock.Object);

        _serviceProviderMock = _autoMocker.GetMock<IServiceProvider>();
		_serviceProviderMock.Setup(x => x.GetService(typeof(MoveMediaProcessorHandler)))
			.Returns(moveMediaProcessorHandler);
        _serviceProviderMock.Setup(x => x.GetService(typeof(MoveLivePhotoProcessorHandler)))
            .Returns(moveLivePhotoProcessorHandler);
        _serviceProviderMock.Setup(x => x.GetService(typeof(MoveAaeProcessorHandler)))
            .Returns(moveAaeProcessorHandler);
        _serviceProviderMock.Setup(x => x.GetService(typeof(MoveXmpProcessorHandler)))
            .Returns(moveXmpProcessorHandler);
        _serviceProviderMock.Setup(x => x.GetService(typeof(CreatedDateAggregatorProcessorHandler)))
            .Returns(createdDateAggregatorProcessor);
    }
	
	[TestCase(MediaType.Image, typeof(MoveMediaProcessorHandler))]
    [TestCase(MediaType.Raw, typeof(MoveMediaProcessorHandler))]
    [TestCase(MediaType.Video, typeof(MoveMediaProcessorHandler))]
    [TestCase(MediaType.WhatsAppImage, typeof(MoveMediaProcessorHandler))]
    [TestCase(MediaType.WhatsAppVideo, typeof(MoveMediaProcessorHandler))]
    [TestCase(MediaType.Insv, typeof(MoveMediaProcessorHandler))]
    public void CreateProcessor_Returns_Successfully(MediaType mediaType, Type processor)
	{
		// Arrange
		var sut = _autoMocker.CreateInstance<ProcessorHandlerFactory>();

		// Act
		var result = sut.CreateProcessorHandler(mediaType);

		// Assert
		result.Should().BeOfType(processor);
    }

    [Test]
	public void CreateProcessor_Throws_Exception()
	{
		// Arrange
		var sut = _autoMocker.CreateInstance<ProcessorHandlerFactory>();

		// Act
		Action act = () => sut.CreateProcessorHandler(MediaType.None);

		// Assert
		act.Should().Throw<FormatException>();
	}
}
