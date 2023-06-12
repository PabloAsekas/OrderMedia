using System;
using FluentAssertions;
using Moq.AutoMock;
using OrderMedia.Services;

namespace OrderMediaTests.Services
{
	public class RandomizerServiceTests
	{
		private AutoMocker _autoMocker;

		[SetUp]
		public void SetUp()
		{
			_autoMocker = new AutoMocker();
		}

		[Test]
		public void GetRandomNumberAsD4_Returns_Number_As_D4()
		{
			// Arrange
			var sut = _autoMocker.CreateInstance<RandomizerService>();

			// Act
			var result = sut.GetRandomNumberAsD4();

			// Assert
			result.Length.Should().Be(4);
		}
	}
}

