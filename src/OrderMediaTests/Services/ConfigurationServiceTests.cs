using Microsoft.Extensions.Configuration;
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
        private string OverwriteFilesName = "OverwriteFiles";
        private bool OverwriteFiles = true;
        private string RenameMediaFilesName = "RenameMediaFiles";
        private bool RenameMediaFiles = true;
        private string ReplaceLongNamesName = "ReplaceLongNames";
        private bool ReplaceLongNames = true;
        private string MaxMediaNameLengthName = "MaxMediaNameLength";
        private int MaxMediaNameLength = 9;
        private string NewMediaName = "NewMediaName";

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
                { OverwriteFilesName, OverwriteFiles.ToString() },
                { RenameMediaFilesName, RenameMediaFiles.ToString() },
                { ReplaceLongNamesName, ReplaceLongNames.ToString() },
                { MaxMediaNameLengthName, MaxMediaNameLength.ToString() },
                { NewMediaName, NewMediaName },
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
			result.Should().Be(MediaSourcePath);
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
            result.Should().Be(ImageFolderName);
        }

        [Test]
        public void GetVideoFolderName_Success()
        {
            // Arrange
            var sut = _autoMocker.CreateInstance<ConfigurationService>();

            // Act
            var result = sut.GetVideoFolderName();

            // Assert
            result.Should().Be(VideoFolderName);
        }

        [Test]
        public void GetRenameMediaFiles_Success()
        {
            // Arrange
            var sut = _autoMocker.CreateInstance<ConfigurationService>();

            // Act
            var result = sut.GetRenameMediaFiles();

            // Assert
            result.Should().Be(RenameMediaFiles);
        }

        [Test]
        public void GetOverwriteFiles_Success()
        {
            // Arrange
            var sut = _autoMocker.CreateInstance<ConfigurationService>();

            // Act
            var result = sut.GetOverwriteFiles();

            // Assert
            result.Should().Be(OverwriteFiles);
        }

        [Test]
        public void GetReplaceLongNames_Success()
        {
            // Arrange
            var sut = _autoMocker.CreateInstance<ConfigurationService>();

            // Act
            var result = sut.GetReplaceLongNames();

            // Assert
            result.Should().Be(ReplaceLongNames);
        }

        [Test]
        public void GetMaxMediaNameLength_Success()
        {
            // Arrange
            var sut = _autoMocker.CreateInstance<ConfigurationService>();

            // Act
            var result = sut.GetMaxMediaNameLength();

            // Assert
            result.Should().Be(MaxMediaNameLength);
        }

        [Test]
        public void GetNewMediaName_Success()
        {
            // Arrange
            var sut = _autoMocker.CreateInstance<ConfigurationService>();

            // Act
            var result = sut.GetNewMediaName();

            // Assert
            result.Should().Be(NewMediaName);
        }
    }
}