using FluentAssertions;
using Microsoft.Extensions.Options;
using OrderMedia.ConsoleApp.Configuration;
using OrderMedia.ConsoleApp.Strategies.FolderStrategy;
using OrderMedia.Enums;

namespace OrderMedia.ConsoleApp.UnitTests.Strategies.FolderStrategy;

public class VideoFolderStrategyTests
{
    private VideoFolderStrategy _sut;
    private const string VideoFolderName = "VideoFolderName";

    [SetUp]
    public void SetUp()
    {
        IOptions<ClassificationSettings> settings = Options.Create(new ClassificationSettings()
        {
            Folders = new ClassificationFolders
            {
                VideoFolderName = VideoFolderName
            }
        });
        
        _sut = new VideoFolderStrategy(settings);
    }

    [TestCase(MediaType.Image, false)]
    [TestCase(MediaType.Raw, false)]
    [TestCase(MediaType.Video, true)]
    [TestCase(MediaType.WhatsAppImage, false)]
    [TestCase(MediaType.WhatsAppVideo, true)]
    [TestCase(MediaType.Insv, true)]
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
        result.Should().Be(VideoFolderName);
    }
}