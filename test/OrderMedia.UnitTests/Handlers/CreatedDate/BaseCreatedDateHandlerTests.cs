using OrderMedia.Handlers.CreatedDate;
using OrderMedia.Interfaces.Handlers;
using OrderMedia.Models;

namespace OrderMedia.UnitTests.Handlers.CreatedDate;

public class BaseCreatedDateHandlerConcrete : BaseCreatedDateHandler
{
    public CreatedDateInfo CreateDateInfoWrapper(string createdDate, string format)
    {
        return CreateCreatedDateInfo(createdDate, format);
    }
}

[TestFixture]
public class BaseCreatedDateHandlerTests
{
    private Mock<ICreatedDateHandler> _nextHandlerMock;
    
    [SetUp]
    public void SetUp()
    {
        _nextHandlerMock = new Mock<ICreatedDateHandler>();
    }

    [Test]
    public void SetNext_Runs_Successfully()
    {
        // Arrange
        var sut = new BaseCreatedDateHandlerConcrete();
        
        // Act
        var result = sut.SetNext(_nextHandlerMock.Object);

        // Assert
        result.Should().Be(_nextHandlerMock.Object);
    }
    
    [Test]
    public void GetCreatedDateInfo_ExecutesNextHandler_Successfully()
    {
        // Arrange
        var createdDateInfo = new CreatedDateInfo
        {
            CreatedDate = "testDate",
            Format = "testFormat",
        };
        const string mediaPath = "test/test.jpg";

        _nextHandlerMock.Setup(x => x.GetCreatedDateInfo(mediaPath))
            .Returns(createdDateInfo);
        
        var sut = new BaseCreatedDateHandlerConcrete();
        sut.SetNext(_nextHandlerMock.Object);
        
        // Act
        var result = sut.GetCreatedDateInfo(mediaPath);

        // Assert
        result.Should().Be(createdDateInfo);
        _nextHandlerMock.Verify(x => x.GetCreatedDateInfo(mediaPath), Times.Once);
    }
    
    [Test]
    public void CreateCreatedDateInfo_ReturnsCreatedDateInfo_Successfully()
    {
        // Arrange
        const string createdDate = "testDate";
        const string format = "testFormat";
        
        var sut = new BaseCreatedDateHandlerConcrete();
        
        // Act
        var result = sut.CreateDateInfoWrapper(createdDate, format);

        // Assert
        result.Should().NotBeNull();
        result.CreatedDate.Should().BeEquivalentTo(createdDate);
        result.Format.Should().BeEquivalentTo(format);
    }
    
    [Test]
    public void CreateCreatedDateInfo_ReturnsNull_Successfully()
    {
        // Arrange
        var createdDate = string.Empty;
        const string format = "testFormat";
        
        var sut = new BaseCreatedDateHandlerConcrete();
        
        // Act
        var result = sut.CreateDateInfoWrapper(createdDate, format);

        // Assert
        result.Should().BeNull();
    }
}