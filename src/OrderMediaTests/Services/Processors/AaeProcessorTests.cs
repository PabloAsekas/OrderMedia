using System;
using Moq;
using Moq.AutoMock;
using OrderMedia.Enums;
using OrderMedia.Interfaces;
using OrderMedia.Models;
using OrderMedia.Services.Processors;

namespace OrderMediaTests.Services.Processors
{
	public class AaeProcessorTests
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
            string aaeName = "IMG_O0001.aae";

            var media = new Media()
            {
                NameWithoutExtension = "IMG_0001",
                MediaFolder = "photos",
                NewMediaFolder = "2014-07-31",
                NewNameWithoutExtension = "2014-07-31_22-15-15_IMG_0001"
            };

            var aaeLocation = $"{media.MediaFolder}/{aaeName}";

            var newAaeName = $"{media.NewNameWithoutExtension}.aae";

            var newAaeLocation = $"{media.NewMediaFolder}/{newAaeName}";

            _renameServiceMock.Setup(x => x.GetAaeName(media.NameWithoutExtension))
                .Returns(aaeName);

            _ioServiceMock.Setup(x => x.Combine(new string[] { media.MediaFolder, aaeName }))
                .Returns(aaeLocation);

            _ioServiceMock.Setup(x => x.Exists(aaeLocation))
                .Returns(true);

            _ioServiceMock.Setup(x => x.Combine(new string[] { media.NewMediaFolder, newAaeName }))
                .Returns(newAaeLocation);

            var sut = _autoMocker.CreateInstance<AaeProcessor>();

            // Act
            sut.Execute(media);

            // Assert
            _ioServiceMock.Verify(x => x.MoveMedia(aaeLocation, newAaeLocation), Times.Once);
        }
    }
}

