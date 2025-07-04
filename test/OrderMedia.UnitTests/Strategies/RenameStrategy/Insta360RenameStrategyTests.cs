using OrderMedia.Interfaces;
using OrderMedia.Strategies.RenameStrategy;

namespace OrderMedia.UnitTests.Strategies.RenameStrategy;

[TestFixture]
public class Insta360RenameStrategyTests
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
    [TestCase("VID_20140731_221515_10_005", ".insv", "VID_2014-07-31_22-15-15_10_005.insv", true)]
    [TestCase("VID_20140731_221515_10_0005", ".insv", "VID_2014-07-31_22-15-15_10_0005.insv", false)]
    [TestCase("VID_20140731_221515_10_0005", ".insv", "VID_2014-07-31_22-15-15_10_pbg_1234.insv", true)]
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
            .Returns(3);
        _configurationService.Setup(x => x.GetNewMediaName())
            .Returns("pbg");

        _randomizerService.Setup(x => x.GetRandomNumberAsD4())
            .Returns("1234");

        var fullName = name + extension;

        var sut = _autoMocker.CreateInstance<Insta360RenameStrategy>();

        // Act
        var result = sut.Rename(fullName, createdDateTime);

        // Assert
        result.Should().Be(renamed);
    }
}