using OrderMedia.Services;

namespace OrderMedia.UnitTests.Services;

[TestFixture]
public class RandomizerServiceTests
{
	[Test]
	public void GetRandomNumberAsD4_Returns_Number_As_D4()
	{
		// Arrange
		var sut = new RandomizerService();

		// Act
		var result = sut.GetRandomNumberAsD4();

		// Assert
		result.Length.Should().Be(4);
	}
}
