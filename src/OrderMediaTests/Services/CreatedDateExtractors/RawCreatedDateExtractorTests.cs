using MetadataExtractor.Formats.Exif;
using OrderMedia.Interfaces;
using OrderMedia.Services.CreatedDateExtractors;

namespace OrderMediaTests.Services.CreatedDateExtractors
{
    public class RawCreatedDateExtractorTests
	{
        private AutoMocker _autoMocker;
        private Mock<IMetadataExtractorService> _metadataExtractorMock;
        private Mock<IIOService> _ioServiceMock;
        private Mock<IXmpExtractorService> _xmpExtractorServiceMock;

        [SetUp]
        public void SetUp()
        {
            _autoMocker = new AutoMocker();

            _metadataExtractorMock = _autoMocker.GetMock<IMetadataExtractorService>();

            _ioServiceMock = _autoMocker.GetMock<IIOService>();

            _xmpExtractorServiceMock = _autoMocker.GetMock<IXmpExtractorService>();
        }

        [Test]
        public void GetCreatedDateTime_FromRawFile_Returns_DateTime()
        {
            // Arrange
            var dateTime = new DateTime(2014, 07, 31, 22, 15, 15);
            var dateTimeAsString = dateTime.ToString("yyyy:MM:dd HH:mm:ss");

            _ioServiceMock.Setup(x => x.FileExists(It.IsAny<string>()))
                .Returns(false);

            _metadataExtractorMock.Setup(x => x.GetRawCreatedDate(It.IsAny<string>()))
                .Returns(dateTimeAsString);

            var sut = _autoMocker.CreateInstance<RawCreatedDateExtractor>();

            // Act
            var result = sut.GetCreatedDateTime("image.raw");

            // Assert
            result.Should().Be(dateTime);
        }

        [Test]
        public void GetCreatedDateTime_FromXmpFile_Returns_DateTime()
        {
            // Arrange
            var mediaFileName = "image";
            var mediaFileExtension = ".raw";
            var mediaFileFolder = "test/";
            var mediaFilePath = $"{mediaFileFolder}{mediaFileName}{mediaFileExtension}";
            var xmpFileName = "image.xmp";
            var xmpFilePath = $"{mediaFileFolder}{xmpFileName}";

            var dateTime = new DateTime(2014, 07, 31, 22, 15, 15);
            var dateTimeAsString = dateTime.ToString("yyyy-MM-ddTHH:mm:ss");

            _ioServiceMock.Setup(x => x.GetDirectoryName(mediaFilePath))
                .Returns(mediaFileFolder);

            _ioServiceMock.Setup(x => x.GetFileNameWithoutExtension(mediaFilePath))
                .Returns(mediaFileName);

            _ioServiceMock.Setup(x => x.Combine(new string[] { mediaFileFolder, xmpFileName }))
                .Returns(xmpFilePath);

            _ioServiceMock.Setup(x => x.FileExists(xmpFilePath))
                .Returns(true);

            _xmpExtractorServiceMock.Setup(x => x.GetCreatedDate(xmpFilePath))
                .Returns(dateTimeAsString);

            var sut = _autoMocker.CreateInstance<RawCreatedDateExtractor>();

            // Act
            var result = sut.GetCreatedDateTime(mediaFilePath);

            // Assert
            result.Should().Be(dateTime);
        }
    }
}

