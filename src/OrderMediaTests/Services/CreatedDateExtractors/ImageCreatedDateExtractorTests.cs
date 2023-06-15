using MetadataExtractor.Formats.Exif;
using OrderMedia.Interfaces;
using OrderMedia.Services.CreatedDateExtractors;

namespace OrderMediaTests.Services.CreatedDateExtractors
{
    public class ImageCreatedDateExtractorTests
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

			var exifSubIfdDirectory = new ExifSubIfdDirectory();
			exifSubIfdDirectory.Set(ExifDirectoryBase.TagDateTimeOriginal, dateTimeAsString);

			directoryList.Add(exifSubIfdDirectory);

			_metadataExtractor.Setup(x => x.GetImageDirectories(It.IsAny<string>()))
				.Returns(directoryList);

			var sut = _autoMocker.CreateInstance<ImageCreatedDateExtractor>();

			// Act
			var result = sut.GetCreatedDateTime("image.jpg");

			// Assert
			result.Should().Be(dateTime);
        }
	}
}

