using OrderMedia.Services.CreatedDateExtractors;

namespace OrderMediaTests.Services.CreatedDateExtractors
{
    public class WhatsAppCreatedDateExtractorTests
	{
        private AutoMocker _autoMocker;

        [SetUp]
        public void SetUp()
        {
            _autoMocker = new AutoMocker();

        }

        [Test]
        public void GetCreatedDateTime_Returns_DateTime()
        {
            // Arrange
            var dateTime = new DateTime(2014, 07, 31, 22, 15, 15);
            var dateTimeAsString = dateTime.ToString("yyyy-MM-dd-HH-mm-ss");

            var path = $"test/PHOTO-{dateTimeAsString}.jpg";

            var sut = _autoMocker.CreateInstance<WhatsAppCreatedDateExtractor>();

            // Act
            var result = sut.GetCreatedDateTime(path);

            // Assert
            result.Should().Be(dateTime);
        }
    }
}

