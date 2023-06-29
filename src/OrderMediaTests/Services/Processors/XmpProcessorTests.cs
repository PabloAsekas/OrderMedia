using OrderMedia.Interfaces;
using OrderMedia.Models;
using OrderMedia.Services.Processors;

namespace OrderMediaTests.Services.Processors
{
    public class XmpProcessorTests
	{
        private AutoMocker _autoMocker;
        private Mock<IIOService> _ioServiceMock;
        private Mock<IRenameService> _renameServiceMock;

        [SetUp]
        public void SetUp()
        {
            _autoMocker = new AutoMocker();

            _ioServiceMock = _autoMocker.GetMock<IIOService>();

            _renameServiceMock = _autoMocker.GetMock<IRenameService>();
        }

        [Test]
        public void Execute_Runs_Succesfully()
        {
            // Arrange
            string xmpName = "IMG_0001.xmp";

            var media = new Media()
            {
                NameWithoutExtension = "IMG_0001",
                MediaFolder = "photos",
                NewMediaFolder = "2014-07-31",
                NewNameWithoutExtension = "2014-07-31_22-15-15_IMG_0001"
            };

            var xmpLocation = $"{media.MediaFolder}/{xmpName}";

            var newXmpName = $"{media.NewNameWithoutExtension}.xmp";

            var newXmpLocation = $"{media.NewMediaFolder}/{newXmpName}";

            _renameServiceMock.Setup(x => x.GetAaeName(media.NameWithoutExtension))
                .Returns(xmpName);

            _ioServiceMock.Setup(x => x.Combine(new string[] { media.MediaFolder, xmpName }))
                .Returns(xmpLocation);

            _ioServiceMock.Setup(x => x.Exists(xmpLocation))
                .Returns(true);

            _ioServiceMock.Setup(x => x.Combine(new string[] { media.NewMediaFolder, newXmpName }))
                .Returns(newXmpLocation);

            var sut = _autoMocker.CreateInstance<XmpProcessor>();

            // Act
            sut.Execute(media);

            // Assert
            _ioServiceMock.Verify(x => x.MoveMedia(xmpLocation, newXmpLocation), Times.Once);
        }
    }
}

