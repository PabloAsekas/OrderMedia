using OrderMedia.Services;

namespace OrderMedia.UnitTests.Services;

[TestFixture]
public class AaeHelperServiceTests
{
	[TestCase("IMG_0001", "IMG_O0001.aae")]
    [TestCase("IMG_0001 (1)", "IMG_0001 (1)O.aae")]
    [TestCase("Test", "Test.aae")]
    public void GetAaeName_Returns_AaeName_Successfully(string nameWithoutExtension, string aaeName)
	{
		// Arrange
		var sut = new AaeHelperService();

		// Act
		var result = sut.GetAaeName(nameWithoutExtension);

		// Assert
		result.Should().Be(aaeName);
	}
}
