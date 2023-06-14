using System;
using System.Globalization;
using FluentAssertions;
using MetadataExtractor.Formats.Exif;
using MetadataExtractor.Formats.QuickTime;
using Moq;
using Moq.AutoMock;
using OrderMedia.Interfaces;
using OrderMedia.Services.CreatedDateExtractors;

namespace OrderMediaTests.Services.CreatedDateExtractors
{
	public class VideoCreatedDateExtractorTests
	{
        private AutoMocker _autoMocker;
        private Mock<IMetadataExtractorService> _metadataExtractor;

        [SetUp]
        public void SetUp()
        {
            _autoMocker = new AutoMocker();

            _metadataExtractor = _autoMocker.GetMock<IMetadataExtractorService>();
        }

        [Test]
        public void GetCreatedDateTime_Returns_DateTime()
        {
            // Arrange
            var dateTime = new DateTime(2014, 07, 31, 22, 15, 15);
            var dateTimeAsString = dateTime.ToString("ddd MMM dd HH:mm:ss zzz yyyy", new CultureInfo("en-UK", false));

            var directoryList = new List<MetadataExtractor.Directory>();

            var quickTimeMetadataHeaderDirectory = new QuickTimeMetadataHeaderDirectory();
            quickTimeMetadataHeaderDirectory.Set(QuickTimeMetadataHeaderDirectory.TagCreationDate, dateTimeAsString);

            directoryList.Add(quickTimeMetadataHeaderDirectory);

            _metadataExtractor.Setup(x => x.GetImageDirectories(It.IsAny<string>()))
                .Returns(directoryList);

            var sut = _autoMocker.CreateInstance<VideoCreatedDateExtractor>();

            // Act
            var result = sut.GetCreatedDateTime("video.mov");

            // Assert
            result.Should().Be(dateTime);
        }
    }
}

