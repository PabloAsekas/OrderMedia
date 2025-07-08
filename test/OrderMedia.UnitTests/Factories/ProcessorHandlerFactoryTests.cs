using Microsoft.Extensions.Options;
using OrderMedia.Enums;
using OrderMedia.Factories;
using OrderMedia.Handlers.Processor;
using OrderMedia.Interfaces;
using OrderMedia.Configuration;

namespace OrderMedia.UnitTests.Factories;

[TestFixture]
public class ProcessorHandlerFactoryTests
{
	private Mock<IServiceProvider> _serviceProviderMock;

	[SetUp]
	public void SetUp()
	{
		var ioWrapperMock = new Mock<IIoWrapper>();
		var aaeHelperServiceMock = new Mock<IAaeHelperService>();
		var metadataAggregatorServiceMock = new Mock<IMetadataAggregatorService>();
		var classificationSettingsOptions = new Mock<IOptions<ClassificationSettingsOptions>>();
		
        var moveMediaProcessorHandler = new MoveMediaProcessorHandler(ioWrapperMock.Object, classificationSettingsOptions.Object);

		var moveLivePhotoProcessorHandler = new MoveLivePhotoProcessorHandler(ioWrapperMock.Object, classificationSettingsOptions.Object);

		var moveAaeProcessorHandler = new MoveAaeProcessorHandler(ioWrapperMock.Object, aaeHelperServiceMock.Object, classificationSettingsOptions.Object);

        var moveXmpProcessorHandler = new MoveXmpProcessorHandler(ioWrapperMock.Object, classificationSettingsOptions.Object);

        var createdDateAggregatorProcessor = new CreatedDateAggregatorProcessorHandler(metadataAggregatorServiceMock.Object);

        _serviceProviderMock = new Mock<IServiceProvider>();
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
		var sut = new ProcessorHandlerFactory(_serviceProviderMock.Object);

		// Act
		var result = sut.CreateProcessorHandler(mediaType);

		// Assert
		result.Should().BeOfType(processor);
    }

    [Test]
	public void CreateProcessor_Throws_Exception()
	{
		// Arrange
		var sut = new ProcessorHandlerFactory(_serviceProviderMock.Object);

		// Act
		Action act = () => sut.CreateProcessorHandler(MediaType.None);

		// Assert
		act.Should().Throw<FormatException>();
	}
}
