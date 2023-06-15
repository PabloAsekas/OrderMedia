using System;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Moq;
using Moq.AutoMock;
using OrderMedia.Interfaces;
using OrderMedia.Services;

namespace OrderMediaTests.Services
{
	public class ConfigurationServiceTests
	{
		private AutoMocker _autoMocker;

        private string MediaSourcePath = "MediaSourcePath";
        private string ImageFolderName = "ImageFolderName";
        private string VideoFolderName = "VideoFolderName";
        private string imageExtensionsName = "ImageExtensions";
        private string[] imageExtensions = new string[] { ".heic", ".jpg", ".jpeg", ".gif", ".png", ".arw", ".dng" };
        private string videoExtensionsName = "VideoExtensions";
        private string[] videoExtensions = new string[] { ".mov", ".mp4" };

        [SetUp]
		public void SetUp()
		{
			_autoMocker = new AutoMocker();

            var appSettings = new Dictionary<string, string>
            {
                { MediaSourcePath, MediaSourcePath },
                { ImageFolderName, ImageFolderName },
                { VideoFolderName, VideoFolderName },
                { $"{imageExtensionsName}:0", imageExtensions[0]},
                { $"{imageExtensionsName}:1", imageExtensions[1]},
                { $"{imageExtensionsName}:2", imageExtensions[2]},
                { $"{imageExtensionsName}:3", imageExtensions[3]},
                { $"{imageExtensionsName}:4", imageExtensions[4]},
                { $"{imageExtensionsName}:5", imageExtensions[5]},
                { $"{imageExtensionsName}:6", imageExtensions[6]},
                { $"{videoExtensionsName}:0", videoExtensions[0]},
                { $"{videoExtensionsName}:1", videoExtensions[1]},
            };

            var configuration = new ConfigurationBuilder()
				.AddInMemoryCollection(appSettings)
				.Build();

			_autoMocker.Use<IConfiguration>(configuration);
		}

		[Test]
		public void GetMediaSourcePath_Success()
		{
			// Arrange
			var sut = _autoMocker.CreateInstance<ConfigurationService>();

			// Act
			var result = sut.GetMediaSourcePath();

			// Assert
			result.Should().BeEquivalentTo(MediaSourcePath);
		}

        [Test]
        public void GetImageExtensions_Success()
        {
            // Arrange
            var sut = _autoMocker.CreateInstance<ConfigurationService>();

            // Act
            var result = sut.GetImageExtensions();

            // Assert
            result.Should().BeEquivalentTo(imageExtensions);
        }

        [Test]
        public void GetVideoExtensions_Success()
        {
            // Arrange
            var sut = _autoMocker.CreateInstance<ConfigurationService>();

            // Act
            var result = sut.GetVideoExtensions();

            // Assert
            result.Should().BeEquivalentTo(videoExtensions);
        }

        [Test]
        public void GetImageFolderName_Success()
        {
            // Arrange
            var sut = _autoMocker.CreateInstance<ConfigurationService>();

            // Act
            var result = sut.GetImageFolderName();

            // Assert
            result.Should().BeEquivalentTo(ImageFolderName);
        }

        [Test]
        public void GetVideoFolderName_Success()
        {
            // Arrange
            var sut = _autoMocker.CreateInstance<ConfigurationService>();

            // Act
            var result = sut.GetVideoFolderName();

            // Assert
            result.Should().BeEquivalentTo(VideoFolderName);
        }
    }
}