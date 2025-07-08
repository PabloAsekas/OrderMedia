using Microsoft.Extensions.Options;
using OrderMedia.Configuration;
using OrderMedia.Interfaces;
using OrderMedia.Strategies.RenameStrategy;

namespace OrderMedia.UnitTests.Strategies.RenameStrategy;

[TestFixture]
public class DefaultRenameStrategyTests
{
    private AutoMocker _autoMocker;
    private Mock<IIoWrapper> _ioServiceMock;
    private Mock<IRandomizerService> _randomizerService;

    [SetUp]
    public void SetUp()
    {
        _autoMocker = new AutoMocker();

        _ioServiceMock = _autoMocker.GetMock<IIoWrapper>();

        _randomizerService = _autoMocker.GetMock<IRandomizerService>();

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

        var classificationSettingsOptions = Options.Create(new ClassificationSettingsOptions
        {
            ReplaceLongNames = replaceLongName,
            MaxMediaNameLength = 8,
            NewMediaName = "pbg"
        });
        
        _autoMocker.Use(classificationSettingsOptions);

        _randomizerService.Setup(x => x.GetRandomNumberAsD4())
            .Returns("1234");

        var fullName = name + extension;

        var sut = _autoMocker.CreateInstance<DefaultRenameStrategy>();

        // Act
        var result = sut.Rename(fullName, createdDateTime);

        // Assert
        result.Should().Be(renamed);
    }
}