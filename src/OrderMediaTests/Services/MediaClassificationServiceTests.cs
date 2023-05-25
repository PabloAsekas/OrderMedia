using System;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.AutoMock;
using OrderMedia.Interfaces;
using OrderMedia.MediaFiles;
using OrderMedia.Services;

namespace OrderMediaTests.Services
{
	public class MediaClassificationServiceTests
	{
		private AutoMocker _autoMocker;
		private Mock<ILogger> _loggerMock;
		private Mock<IIOService> _ioServiceMock;
		private Mock<IConfigurationService> _configurationServiceMock;
		private Mock<BaseMedia> _mediaMock;
		private Mock<IMediaFactoryService> _mediaFactoryServiceMock;

		private List<FileInfo> _files;

		[SetUp]
		public void SetUp()
		{
            _autoMocker = new AutoMocker();

			_files = new List<FileInfo>()
			{
				new FileInfo("test1"),
				new FileInfo("test2")
			};

			_loggerMock = _autoMocker.GetMock<ILogger>();

			_ioServiceMock = _autoMocker.GetMock<IIOService>();
			_ioServiceMock.Setup(x => x.GetFilesByExtensions(It.IsAny<string>(), It.IsAny<string>()))
				.Returns(_files);

            _configurationServiceMock = _autoMocker.GetMock<IConfigurationService>();

			_mediaMock = _autoMocker.GetMock<BaseMedia>();

            _mediaFactoryServiceMock = _autoMocker.GetMock<IMediaFactoryService>();
			_mediaFactoryServiceMock.Setup(x => x.CreateMedia(It.IsAny<string>()))
				.Returns(_mediaMock.Object);
        }

		[Test]
		public async Task StartAsync_Executes()
		{
			// Arrange
			var sut = _autoMocker.CreateInstance<MediaClassificationService>();

			// Act
			await sut.StartAsync(new CancellationToken());

			// Assert
			_loggerMock.Verify();
			_ioServiceMock.Verify();
			_mediaMock.Verify();
		}
    }
}

