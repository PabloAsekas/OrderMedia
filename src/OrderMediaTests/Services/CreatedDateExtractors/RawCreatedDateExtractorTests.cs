using System;
using FluentAssertions;
using MetadataExtractor.Formats.Exif;
using Moq;
using Moq.AutoMock;
using OrderMedia.Interfaces;
using OrderMedia.Services.CreatedDateExtractors;

namespace OrderMediaTests.Services.CreatedDateExtractors
{
	public class RawCreatedDateExtractorTests
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
            var dateTimeAsString = dateTime.ToString("yyyy:MM:dd HH:mm:ss");

            var directoryList = new List<MetadataExtractor.Directory>();

            var exifIfd0Directory = new ExifIfd0Directory();
            exifIfd0Directory.Set(ExifIfd0Directory.TagDateTime, dateTimeAsString);

            directoryList.Add(exifIfd0Directory);

            _metadataExtractor.Setup(x => x.GetImageDirectories(It.IsAny<string>()))
                .Returns(directoryList);

            var sut = _autoMocker.CreateInstance<RawCreatedDateExtractor>();

            // Act
            var result = sut.GetCreatedDateTime("image.raw");

            // Assert
            result.Should().Be(dateTime);
        }
    }
}

