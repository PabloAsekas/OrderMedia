using OrderMedia.Interfaces;
using OrderMedia.Services.CreatedDateExtractors;

namespace OrderMediaTests.Services.CreatedDateExtractors
{
    public class WhatsAppCreatedDateExtractorTests
	{
        private AutoMocker _autoMocker;
        private Mock<IIOService> _ioServiceMock;

        [SetUp]
        public void SetUp()
        {
            _autoMocker = new AutoMocker();

            _ioServiceMock = _autoMocker.GetMock<IIOService>();
        }

        [Test]
        public void GetCreatedDateTime_Returns_DateTime()
        {
            // Arrange
            var dateTime = new DateTime(2014, 07, 31, 22, 15, 15);
            var dateTimeAsString = dateTime.ToString("yyyy-MM-dd-HH-mm-ss");

            var mediaName = $"PHOTO-{dateTimeAsString}.jpg";

            var path = $"test/{mediaName}";

            _ioServiceMock.Setup(x => x.GetFileNameWithoutExtension(It.IsAny<string>()))
                .Returns(mediaName);

            var sut = _autoMocker.CreateInstance<WhatsAppCreatedDateExtractor>();

            // Act
            var result = sut.GetCreatedDateTime(path);

            // Assert
            result.Should().Be(dateTime);
        }
    }
}

