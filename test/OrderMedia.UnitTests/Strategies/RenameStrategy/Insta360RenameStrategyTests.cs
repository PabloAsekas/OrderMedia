using OrderMedia.Interfaces;
using OrderMedia.Models;
using OrderMedia.Strategies.RenameStrategy;

namespace OrderMedia.UnitTests.Strategies.RenameStrategy;

[TestFixture]
public class Insta360RenameStrategyTests
{
    private Mock<IIoWrapper> _ioWrapperMock;
    private Mock<IRandomizerService> _randomizerService;
    
    [SetUp]
    public void SetUp()
    {
        _ioWrapperMock = new Mock<IIoWrapper>();

        _randomizerService = new Mock<IRandomizerService>();
    }
    
    [Test]
    [TestCase("VID_20140731_221515_10_005", ".insv", "VID_2014-07-31_22-15-15_10_005.insv", true)]
    [TestCase("VID_20140731_221515_10_0005", ".insv", "VID_2014-07-31_22-15-15_10_0005.insv", false)]
    [TestCase("VID_20140731_221515_10_0005", ".insv", "VID_2014-07-31_22-15-15_10_pbg_1234.insv", true)]
    public void Rename_Returns_Name_Successfully(string name, string extension, string renamed, bool replaceLongName)
    {
        // Arrange
        var createdDateTime = new DateTime(2014, 7, 31, 22, 15, 15);

        var fullName = name + extension;

        var request = new RenameMediaRequest
        {
            Name = fullName,
            CreatedDate = createdDateTime,
            ReplaceName = replaceLongName,
            MaximumNameLength = 3,
            NewName = "pbg"
        };
        
        _ioWrapperMock.Setup(x => x.GetExtension(It.IsAny<string>()))
            .Returns(extension);
        _ioWrapperMock.Setup(x => x.GetFileNameWithoutExtension(It.IsAny<string>()))
            .Returns(name);
        
        _randomizerService.Setup(x => x.GetRandomNumberAsD4())
            .Returns("1234");


        var sut = new Insta360RenameStrategy(_ioWrapperMock.Object, _randomizerService.Object);

        // Act
        var result = sut.Rename(request);

        // Assert
        result.Should().Be(renamed);
    }
}