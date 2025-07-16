using Microsoft.Extensions.Options;
using OrderMedia.Configuration;
using OrderMedia.Enums;
using OrderMedia.Factories;
using OrderMedia.Handlers.Processor;
using OrderMedia.Interfaces;
using OrderMedia.Interfaces.Factories;

namespace OrderMedia.UnitTests.Factories;

public class ProcessorChainFactoryTests
{
    private Mock<IServiceProvider> _serviceProviderMock;
    private IReadOnlyDictionary<string, IProcessorHandlerFactory> _handlers;
    private IOptions<ClassificationProcessorsOptions> _options;

    [SetUp]
    public void SetUp()
    {
        _serviceProviderMock = new Mock<IServiceProvider>();
        
        _handlers = new Dictionary<string, IProcessorHandlerFactory>();
    }

    [Test]
    public void Build_ReturnsNull_WhenNoProcessorsConfigured_Successfully()
    {
        // Arrange
        var options = Options.Create(new ClassificationProcessorsOptions());

        var sut = new ProcessorChainFactory(_serviceProviderMock.Object, _handlers, options);
        
        // Act
        var result = sut.Build(MediaType.None);

        // Assert
        result.Should().BeNull();
    }
    
    [Test]
    public void Build_ReturnsNull_WhenProcessorsDoesNotExist_Successfully()
    {
        // Arrange
        var options = Options.Create(new ClassificationProcessorsOptions
        {
            Processors = new Dictionary<string, List<string>>
            {
                {"Test", [] }
            }
        });

        var sut = new ProcessorChainFactory(_serviceProviderMock.Object, _handlers, options);
        
        // Act
        var result = sut.Build(MediaType.None);

        // Assert
        result.Should().BeNull();
    }

    [Test]
    public void Build_ReturnsChain_WhenOneProcessorInList_Successfully()
    {
        // Arrange
        var iIoWrapperMock = new Mock<IIoWrapper>();
        var classificationSettingsOptionsMock = new Mock<IOptions<ClassificationSettingsOptions>>();
        var moveMediaProcessorHandlerMock = new MoveMediaProcessorHandler(iIoWrapperMock.Object, classificationSettingsOptionsMock.Object);
        var factory = new Mock<IProcessorHandlerFactory>();
        factory.Setup(x => x.CreateInstance(It.IsAny<IServiceProvider>()))
            .Returns(moveMediaProcessorHandlerMock);
        
        IReadOnlyDictionary<string, IProcessorHandlerFactory> handlers =
            new Dictionary<string, IProcessorHandlerFactory>
            {
                {"MoveMediaProcessorHandler", factory.Object}
            };
        
        var options = Options.Create(new ClassificationProcessorsOptions
        {
            Processors = new Dictionary<string, List<string>>
            {
                {"Image", ["MoveMediaProcessorHandler"] }
            }
        });
        
        var sut = new ProcessorChainFactory(_serviceProviderMock.Object, handlers, options);
        
        // Act
        var result = sut.Build(MediaType.Image);
        
        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<MoveMediaProcessorHandler>();
    }
    
    [Test]
    public void Build_ReturnsChain_WhenMultipleProcessorsInList_Successfully()
    {
        // Arrange
        var iIoWrapperMock = new Mock<IIoWrapper>();
        var classificationSettingsOptionsMock = new Mock<IOptions<ClassificationSettingsOptions>>();
        var moveMediaProcessorHandlerMock = new MoveMediaProcessorHandler(iIoWrapperMock.Object, classificationSettingsOptionsMock.Object);
        var factory = new Mock<IProcessorHandlerFactory>();
        factory.Setup(x => x.CreateInstance(It.IsAny<IServiceProvider>()))
            .Returns(moveMediaProcessorHandlerMock);
        
        IReadOnlyDictionary<string, IProcessorHandlerFactory> handlers =
            new Dictionary<string, IProcessorHandlerFactory>
            {
                {"MoveMediaProcessorHandler", factory.Object}
            };
        
        var options = Options.Create(new ClassificationProcessorsOptions
        {
            Processors = new Dictionary<string, List<string>>
            {
                {"Raw", ["MoveAaeProcessorHandler"] },
                {"Video", ["MoveMediaProcessorHandler"] },
                {"Image", ["MoveMediaProcessorHandler"] },
            }
        });
        
        var sut = new ProcessorChainFactory(_serviceProviderMock.Object, handlers, options);
        
        // Act
        var result = sut.Build(MediaType.Image);
        
        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<MoveMediaProcessorHandler>();
    }
    
    [Test]
    public void Build_ReturnsChain_WhenMultipleProcessorsPerMediaType_Successfully()
    {
        // Arrange
        var iIoWrapperMock = new Mock<IIoWrapper>();
        var classificationSettingsOptionsMock = new Mock<IOptions<ClassificationSettingsOptions>>();
        var moveMediaProcessorHandler = new MoveMediaProcessorHandler(iIoWrapperMock.Object, classificationSettingsOptionsMock.Object);
        var factory1 = new Mock<IProcessorHandlerFactory>();
        factory1.Setup(x => x.CreateInstance(It.IsAny<IServiceProvider>()))
            .Returns(moveMediaProcessorHandler); 
        
        var iMetadataAggregatorServiceMock = new Mock<IMetadataAggregatorService>();
        var createdDateAggregatorProcessorHandler = new CreatedDateAggregatorProcessorHandler(iMetadataAggregatorServiceMock.Object);
        var factory2 = new Mock<IProcessorHandlerFactory>();
        factory2.Setup(x => x.CreateInstance(It.IsAny<IServiceProvider>()))
            .Returns(createdDateAggregatorProcessorHandler);
        
        
        IReadOnlyDictionary<string, IProcessorHandlerFactory> handlers =
            new Dictionary<string, IProcessorHandlerFactory>
            {
                {"MoveMediaProcessorHandler", factory1.Object},
                {"CreatedDateAggregatorProcessorHandler", factory2.Object}
            };
        
        var options = Options.Create(new ClassificationProcessorsOptions
        {
            Processors = new Dictionary<string, List<string>>
            {
                {"Raw", ["MoveAaeProcessorHandler"] },
                {"Image", ["CreatedDateAggregatorProcessorHandler", "MoveMediaProcessorHandler"] },
            }
        });
        
        var sut = new ProcessorChainFactory(_serviceProviderMock.Object, handlers, options);
        
        // Act
        var result = sut.Build(MediaType.Image);
        
        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<CreatedDateAggregatorProcessorHandler>();
    }
}