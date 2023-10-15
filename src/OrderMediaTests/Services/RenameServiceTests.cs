using OrderMedia.Interfaces;
using OrderMedia.Services;

namespace OrderMediaTests.Services
{
    public class RenameServiceTests
	{
		private AutoMocker _autoMocker;
		private Mock<IIOService> _ioServiceMock;
		private Mock<IRandomizerService> _randomizerService;
		private Mock<IConfigurationService> _configurationService;

		[SetUp]
		public void SetUp()
		{
			_autoMocker = new AutoMocker();

			_ioServiceMock = _autoMocker.GetMock<IIOService>();

			_randomizerService = _autoMocker.GetMock<IRandomizerService>();

			_configurationService = _autoMocker.GetMock<IConfigurationService>();
		}

        [Test]
        [TestCase("IMG_0001", ".jpg", "2014-07-31_22-15-15_IMG_0001.jpg", true)]
        [TestCase("IMG_0001 (1)", ".jpg", "2014-07-31_22-15-15_IMG_0001.jpg", true)]
		[TestCase("IMG_00001", ".jpg", "2014-07-31_22-15-15_pbg_1234.jpg", true)]
        [TestCase("IMG_00001", ".jpg", "2014-07-31_22-15-15_IMG_00001.jpg", false)]
        public void Rename_Returns_Name_Successfully(string name, string extension, string renamed, bool replaceLongName)
        {
			// Arrange
			var createdDateTime = new DateTime(2014, 7, 31, 22, 15, 15);

			_ioServiceMock.Setup(x => x.GetExtension(It.IsAny<string>()))
				.Returns(extension);
			_ioServiceMock.Setup(x => x.GetFileNameWithoutExtension(It.IsAny<string>()))
				.Returns(name);

			_configurationService.Setup(x => x.GetReplaceLongNames())
				.Returns(replaceLongName);
			_configurationService.Setup(x => x.GetMaxMediaNameLength())
				.Returns(8);
			_configurationService.Setup(x => x.GetNewMediaName())
				.Returns("pbg");

			_randomizerService.Setup(x => x.GetRandomNumberAsD4())
				.Returns("1234");

			var fullName = name + extension;

            var sut = _autoMocker.CreateInstance<RenameService>();

            // Act
            var result = sut.Rename(fullName, createdDateTime);

            // Assert
            result.Should().Be(renamed);
        }

        [Test]
		[TestCase("IMG_0001", "IMG_O0001.aae")]
        [TestCase("IMG_0001 (1)", "IMG_0001 (1)O.aae")]
        [TestCase("Test", "Test.aae")]
        public void GetAaeName_Returns_AaeName_Successfully(string nameWithoutExtension, string aaeName)
		{
			// Arrange
			var sut = _autoMocker.CreateInstance<RenameService>();

			// Act
			var result = sut.GetAaeName(nameWithoutExtension);

			// Assert
			result.Should().Be(aaeName);
		}
	}
}

