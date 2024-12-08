using OrderMedia.Services;

namespace OrderMedia.UnitTests.Services
{
    public class AaeHelperServiceTests
	{
		private AutoMocker _autoMocker;

		[SetUp]
		public void SetUp()
		{
			_autoMocker = new AutoMocker();
		}

        [Test]
		[TestCase("IMG_0001", "IMG_O0001.aae")]
        [TestCase("IMG_0001 (1)", "IMG_0001 (1)O.aae")]
        [TestCase("Test", "Test.aae")]
        public void GetAaeName_Returns_AaeName_Successfully(string nameWithoutExtension, string aaeName)
		{
			// Arrange
			var sut = _autoMocker.CreateInstance<AaeHelperService>();

			// Act
			var result = sut.GetAaeName(nameWithoutExtension);

			// Assert
			result.Should().Be(aaeName);
		}
	}
}

