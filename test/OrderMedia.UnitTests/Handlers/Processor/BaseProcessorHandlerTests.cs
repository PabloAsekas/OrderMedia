using OrderMedia.Handlers.Processor;
using OrderMedia.Interfaces.Handlers;
using OrderMedia.Models;

namespace OrderMedia.UnitTests.Handlers.Processor;

public class BaseProcessorHandlerConcrete : BaseProcessorHandler
{
}

[TestFixture]
public class BaseProcessorHandlerTests
{
    
    private AutoMocker _autoMocker;
    private Mock<IProcessorHandler> _nextHandlerMock;

    [SetUp]
    public void SetUp()
    {
        _autoMocker = new AutoMocker();

        _nextHandlerMock = _autoMocker.GetMock<IProcessorHandler>();
    }
    
    [Test]
    public void SetNext_Runs_Successfully()
    {
        // Arrange
        var sut = _autoMocker.CreateInstance<BaseProcessorHandlerConcrete>();
        
        // Act
        var result = sut.SetNext(_nextHandlerMock.Object);

        // Assert
        result.Should().Be(_nextHandlerMock.Object);
    }
    
    [Test]
    public void Process_ExecutesNextHandler_Successfully()
    {
        // Arrange
        var media = new Media();
        
        var sut = _autoMocker.CreateInstance<BaseProcessorHandlerConcrete>();
        sut.SetNext(_nextHandlerMock.Object);
        
        // Act
        sut.Process(media);

        // Assert
        _nextHandlerMock.Verify(x => x.Process(media), Times.Once);
    }
}