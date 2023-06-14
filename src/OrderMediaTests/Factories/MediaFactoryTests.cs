using System;
using FluentAssertions;
using Moq;
using Moq.AutoMock;
using OrderMedia.Enums;
using OrderMedia.Factories;
using OrderMedia.Interfaces;
using OrderMedia.Models;
using OrderMedia.Services.CreatedDateExtractors;

namespace OrderMediaTests.Factories
{
    public class MediaFactoryTests
    {
        private AutoMocker _autoMocker;

        private Mock<IConfigurationService> _configurationServiceMock;
        private Mock<IIOService> _ioServiceMock;
        private Mock<IRenameService> _renameServiceMock;
        private Mock<IMediaTypeService> _mediaTypeServiceMock;
        private Mock<ICreatedDateExtractorsFactory> _createdDateTimeServiceFactoryMock;

        private const string mediaFolder = "path";
        private const string mediaPath = $"test/{mediaFolder}/";
        private const string nameWithoutExtension = "media";
        private const string image = $"{nameWithoutExtension}.jpg";
        private const string raw = $"{nameWithoutExtension}.dng";
        private const string video = $"{nameWithoutExtension}.mp4";
        private const string whatsApp = $"{nameWithoutExtension}.whatsapp";
        private const string classificationFolderName = "img";
        private const string createdDate = "2014-07-31";
        private static DateTime createdDateTime = new DateTime(2014, 7, 31, 22, 15, 15);
        private const string newMediaFolder = $"{mediaFolder}/{classificationFolderName}/{createdDate}";
        private const string newNameWithoutExtension = "renamedName";
        private const string newName = $"{newNameWithoutExtension}.jpg";
        private const string newMediaPath = $"{newMediaFolder}/{newName}";

        [SetUp]
        public void SetUp()
        {
            _autoMocker = new AutoMocker();

            _configurationServiceMock = _autoMocker.GetMock<IConfigurationService>();
            _configurationServiceMock.Setup(x => x.GetImageFolderName())
                .Returns(classificationFolderName);
            _configurationServiceMock.Setup(x => x.GetVideoFolderName())
                .Returns(classificationFolderName);

            _ioServiceMock = _autoMocker.GetMock<IIOService>();
            _ioServiceMock.Setup(x => x.GetDirectoryName(It.IsAny<string>()))
                .Returns(mediaFolder);
            _ioServiceMock.Setup(x => x.GetFileName(mediaPath + image))
                .Returns(image);
            _ioServiceMock.Setup(x => x.GetFileName(mediaPath + raw))
                .Returns(raw);
            _ioServiceMock.Setup(x => x.GetFileName(mediaPath + video))
                .Returns(video);
            _ioServiceMock.Setup(x => x.GetFileName(mediaPath + whatsApp))
                .Returns(whatsApp);
            _ioServiceMock.Setup(x => x.GetFileNameWithoutExtension(mediaPath + image))
                .Returns(nameWithoutExtension);
            _ioServiceMock.Setup(x => x.GetFileNameWithoutExtension(mediaPath + raw))
                .Returns(nameWithoutExtension);
            _ioServiceMock.Setup(x => x.GetFileNameWithoutExtension(mediaPath + video))
                .Returns(nameWithoutExtension);
            _ioServiceMock.Setup(x => x.GetFileNameWithoutExtension(mediaPath + whatsApp))
                .Returns(nameWithoutExtension);
            _ioServiceMock.Setup(x => x.GetFileNameWithoutExtension(newName))
                .Returns(newNameWithoutExtension);
            _ioServiceMock.Setup(x => x.Combine(new string[] { mediaFolder, classificationFolderName, createdDateTime.ToString("yyyy-MM-dd") }))
                .Returns(newMediaFolder);
            _ioServiceMock.Setup(x => x.Combine(new string[] { newMediaFolder, newName }))
                .Returns(newMediaPath);

            _renameServiceMock = _autoMocker.GetMock<IRenameService>();
            _renameServiceMock.Setup(x => x.Rename(It.IsAny<string>(), It.IsAny<DateTime>()))
                .Returns(newName);

            _mediaTypeServiceMock = _autoMocker.GetMock<IMediaTypeService>();
            _mediaTypeServiceMock.Setup(x => x.GetMediaType(mediaPath + image))
                .Returns(MediaType.Image);
            _mediaTypeServiceMock.Setup(x => x.GetMediaType(mediaPath + raw))
                .Returns(MediaType.Raw);
            _mediaTypeServiceMock.Setup(x => x.GetMediaType(mediaPath + video))
                .Returns(MediaType.Video);
            _mediaTypeServiceMock.Setup(x => x.GetMediaType(mediaPath + whatsApp))
                .Returns(MediaType.WhatsApp);

            var createdDateExtractor = _autoMocker.GetMock<ICreatedDateExtractor>();
            createdDateExtractor.Setup(x => x.GetCreatedDateTime(It.IsAny<string>()))
                .Returns(createdDateTime);

            _createdDateTimeServiceFactoryMock = _autoMocker.GetMock<ICreatedDateExtractorsFactory>();
            _createdDateTimeServiceFactoryMock.Setup(x => x.GetExtractor(It.IsAny<MediaType>()))
                .Returns(createdDateExtractor.Object);
        }

        [Test]
        [TestCase(image, MediaType.Image)]
        [TestCase(raw, MediaType.Raw)]
        [TestCase(video, MediaType.Video)]
        [TestCase(whatsApp, MediaType.WhatsApp)]
        public void CreateMedia_Returns_Succesfully(string name, MediaType mediaType)
        {
            // Arrange
            string fullPath = mediaPath + name;

            var sut = _autoMocker.CreateInstance<MediaFactory>();

            // Act
            var result = sut.CreateMedia(fullPath);

            // Assert
            result.Should().BeOfType<Media>();
            result.MediaType.Should().Be(mediaType);
            result.MediaPath.Should().Be(fullPath);
            result.MediaFolder.Should().Be(mediaFolder);
            result.Name.Should().Be(name);
            result.NameWithoutExtension.Should().Be(nameWithoutExtension);
            result.CreatedDateTime.Should().Be(createdDateTime);
            result.NewMediaPath.Should().Be(newMediaPath);
            result.NewMediaFolder.Should().Be(newMediaFolder);
            result.NewName.Should().Be(newName);
            result.NewNameWithoutExtension.Should().Be(newNameWithoutExtension);
        }

        [Test]
        public void CreateMedia_Throws_Exception()
        {
            // Arrange
            string fullPath = mediaPath + "none.none";

            _mediaTypeServiceMock.Setup(x => x.GetMediaType(fullPath))
                .Returns(MediaType.None);

            var sut = _autoMocker.CreateInstance<MediaFactory>();

            // Act
            Action act = () => sut.CreateMedia(fullPath);

            // Assert
            act.Should().Throw<FormatException>();
        }
    }
}