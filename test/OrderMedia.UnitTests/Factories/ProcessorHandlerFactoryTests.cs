using OrderMedia.Factories;
using OrderMedia.Interfaces.Handlers;
using OrderMedia.UnitTests.Handlers.Processor;

namespace OrderMedia.UnitTests.Factories;

[TestFixture]
public class ProcessorHandlerFactoryTests
{
	private Mock<IServiceProvider> _serviceProviderMock;
	private Mock<Func<IServiceProvider, IProcessorHandler>> _factory;

	[SetUp]
	public void SetUp()
	{
		_serviceProviderMock = new Mock<IServiceProvider>();
		
		_factory = new Mock<Func<IServiceProvider, IProcessorHandler>>();
		_factory.Setup(x => x.Invoke(It.IsAny<IServiceProvider>()))
			.Returns(new BaseProcessorHandlerConcrete());
    }

	[Test]
	public void CreateInstance_ExecutesFactory_Successfully()
	{
		// Arrange
		var sut = new ProcessorHandlerFactory(_factory.Object);
		
		// Act
		var result = sut.CreateInstance(_serviceProviderMock.Object);
		
		// Assert
		result.Should().BeOfType<BaseProcessorHandlerConcrete>();
		_factory.Verify(x => x.Invoke(It.IsAny<IServiceProvider>()), Times.Once);
	}
}
