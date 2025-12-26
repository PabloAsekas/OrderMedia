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
    private Mock<IProcessorHandler> _nextHandlerMock;

    [SetUp]
    public void SetUp()
    {
        _nextHandlerMock = new Mock<IProcessorHandler>();
    }
    
    [Test]
    public void SetNext_Runs_Successfully()
    {
        // Arrange
        var sut = new BaseProcessorHandlerConcrete();
        
        // Act
        var result = sut.SetNext(_nextHandlerMock.Object);

        // Assert
        result.Should().Be(_nextHandlerMock.Object);
    }
    
    [Test]
    public void Process_ExecutesNextHandler_Successfully()
    {
        // Arrange
        var request = new ProcessMediaRequest();
        
        var sut = new BaseProcessorHandlerConcrete();
        sut.SetNext(_nextHandlerMock.Object);
        
        // Act
        sut.Process(request);

        // Assert
        _nextHandlerMock.Verify(x => x.Process(request), Times.Once);
    }
}