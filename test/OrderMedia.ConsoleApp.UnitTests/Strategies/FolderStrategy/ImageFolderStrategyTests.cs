using FluentAssertions;
using Microsoft.Extensions.Options;
using OrderMedia.ConsoleApp.Configuration;
using OrderMedia.ConsoleApp.Strategies.FolderStrategy;
using OrderMedia.Enums;

namespace OrderMedia.ConsoleApp.UnitTests.Strategies.FolderStrategy;

public class ImageFolderStrategyTests
{
    private ImageFolderStrategy _sut;
    private const string ImageFolderName = "ImageFolderName";

    [SetUp]
    public void SetUp()
    {
        IOptions<ClassificationSettings> settings = Options.Create(new ClassificationSettings()
        {
            Folders = new ClassificationFolders
            {
                ImageFolderName = ImageFolderName
            }
        });
        
        _sut = new ImageFolderStrategy(settings);
    }

    [TestCase(MediaType.Image, true)]
    [TestCase(MediaType.Raw, true)]
    [TestCase(MediaType.Video, false)]
    [TestCase(MediaType.WhatsAppImage, true)]
    [TestCase(MediaType.WhatsAppVideo, false)]
    [TestCase(MediaType.Insv, false)]
    public void CanHandle_ReturnsExpectedResult(MediaType type, bool expected)
    {
        // Arrange
        
        // Act
        var result = _sut.CanHandle(type);
        
        // Assert
        result.Should().Be(expected);
    }

    [Test]
    public void GetTargetFolder_ReturnsTargetFolder_WhenConfigurationIsValid()
    {
        // Arrange
        
        // Act
        var result = _sut.GetTargetFolder();
        
        // Assert
        result.Should().Be(ImageFolderName);
    }
}