using FluentAssertions;
using OrderMedia.ConsoleApp.Services;
using OrderMedia.Models;

namespace OrderMedia.ConsoleApp.UnitTests.Services;

[TestFixture]
public class RenamingValidatorServiceTests
{
    [Test]
    public void ValidateMedia_ReturnsTrue_WhenMediaIsValid()
    {
        // Arrange
        var media = new Media
        {
            CreatedDateTime = new DateTime(2014, 07, 31, 22, 15, 00),
            Name = "test.jpg",
            NameWithoutExtension = "test",
        };

        var sut = new RenamingValidatorService();
        
        // Act
        var result = sut.ValidateMedia(media);
        
        // Assert
        result.Should().BeTrue();
    }
    
    [Test]
    public void ValidateMedia_ReturnsFalse_WhenCreatedDateTimeIsDefault()
    {
        // Arrange
        var media = new Media
        {
            CreatedDateTime = default
        };

        var sut = new RenamingValidatorService();
        
        // Act
        var result = sut.ValidateMedia(media);
        
        // Assert
        result.Should().BeFalse();
    }
    
    [TestCase("-HDR")]
    [TestCase("-Pano")]
    public void ValidateMedia_ReturnsFalse_WhenMediaNameEndsWith(string endsWith)
    {
        // Arrange
        var media = new Media
        {
            CreatedDateTime = default,
            Name = $"test{endsWith}.jpg"
        };

        var sut = new RenamingValidatorService();
        
        // Act
        var result = sut.ValidateMedia(media);
        
        // Assert
        result.Should().BeFalse();
    }
    
    [TestCase("-HDR-")]
    [TestCase("-Pano-")]
    public void ValidateMedia_ReturnsFalse_WhenMediaNameContains(string contains)
    {
        // Arrange
        var media = new Media
        {
            CreatedDateTime = default,
            Name = $"test{contains}2.jpg"
        };

        var sut = new RenamingValidatorService();
        
        // Act
        var result = sut.ValidateMedia(media);
        
        // Assert
        result.Should().BeFalse();
    }
}