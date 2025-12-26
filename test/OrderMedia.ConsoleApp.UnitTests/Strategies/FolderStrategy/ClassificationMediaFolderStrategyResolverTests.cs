using FluentAssertions;
using Moq;
using OrderMedia.ConsoleApp.Interfaces;
using OrderMedia.ConsoleApp.Strategies.FolderStrategy;
using OrderMedia.Enums;

namespace OrderMedia.ConsoleApp.UnitTests.Strategies.FolderStrategy;

public class ClassificationMediaFolderStrategyResolverTests
{
    [Test]
    public void Resolve_ReturnsStrategy_WhenThereIsStrategyRegistered()
    {
        // Arrange
        var strategyMock = new Mock<IClassificationMediaFolderStrategy>();
        
        strategyMock.Setup(x => x.CanHandle(It.IsAny<MediaType>())).Returns(true);
        
        var sut = new ClassificationMediaFolderStrategyResolver(new List<IClassificationMediaFolderStrategy> { strategyMock.Object });

        // Act
        var result = sut.Resolve(MediaType.Image);
        
        // Assert
        result.Should().BeAssignableTo<IClassificationMediaFolderStrategy>();
    }

    [Test]
    public void Resolve_ThrowsException_WhenThereIsNoStrategyRegistered()
    {
        // Arrange
        var sut = new ClassificationMediaFolderStrategyResolver(new List<IClassificationMediaFolderStrategy>());
        
        // Act
        Action act =  () => sut.Resolve(MediaType.Image);
        
        // Assert
        act.Should().Throw<NotSupportedException>();
    }
}