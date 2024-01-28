using OrderMedia.Interfaces;
using OrderMedia.Models;
using OrderMedia.Services.Processors;

namespace OrderMediaTests.Services.Processors
{
    public class MainProcessorTests
	{
        private AutoMocker _autoMocker;
        private Mock<IIOService> _ioServiceMock;

        [SetUp]
        public void SetUp()
        {
            _autoMocker = new AutoMocker();

            _ioServiceMock = _autoMocker.GetMock<IIOService>();
        }

        [Test]
        public void Execute_Runs_Successfully()
        {
            // Arrange
            var media = new Media()
            {
                NewMediaFolder = "/2014-07-31/",
                MediaPath = "test/photos/IMG_0001.jpg",
                NewMediaPath = "test/photos/img/2014-07-31/2014-07-31_22-15-15_IMG_0001.jpg",
                CreatedDateTime = new DateTime(2014, 07, 31, 22, 15, 15),
            };

            var sut = _autoMocker.CreateInstance<MainProcessor>();

            // Act
            sut.Execute(media);

            // Assert
            _ioServiceMock.Verify(x => x.CreateFolder(media.NewMediaFolder), Times.Once);
            _ioServiceMock.Verify(x => x.MoveMedia(media.MediaPath, media.NewMediaPath), Times.Once);
        }

        [Test]
        public void Execute_Runs_WithNoClassifiableMedia()
        {
            // Arrange
            var media = new Media()
            {
                NewMediaFolder = "/2014-07-31/",
                MediaPath = "test/photos/IMG_0001.jpg",
                NewMediaPath = "test/photos/img/2014-07-31/2014-07-31_22-15-15_IMG_0001.jpg",
            };

            var sut = _autoMocker.CreateInstance<MainProcessor>();

            // Act
            sut.Execute(media);

            // Assert
            _ioServiceMock.Verify(x => x.CreateFolder(media.NewMediaFolder), Times.Never);
            _ioServiceMock.Verify(x => x.MoveMedia(media.MediaPath, media.NewMediaPath), Times.Never);
        }
    }
}

