using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Containers;
using DotNet.Testcontainers.Images;
using FluentAssertions;

namespace OrderMedia.IntegrationTests;

[TestFixture]
public class MediaClassificationIntegrationTests
{
    private readonly string _mediaPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "media");
    private readonly string _newMediaPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "testmedia");
    private IFutureDockerImage _dockerImage;
    private IContainer _container;
    
    [OneTimeSetUp]
    public async Task CreateInitialSetup()
    {
        if (Directory.Exists(_newMediaPath))
        {
            Directory.Delete(_newMediaPath, true);
        }
        
        CopyDirectory(_mediaPath, _newMediaPath, false);
        
        // Build image with OrderMedia.ConsoleApp.IntegrationTests Dockerfile. It has rights for appsettings.json and just one stage
        _dockerImage = new ImageFromDockerfileBuilder()
            .WithBuildArgument("RESOURCE_REAPER_SESSION_ID", ResourceReaper.DefaultSessionId.ToString("D"))
            .WithDockerfileDirectory(CommonDirectoryPath.GetSolutionDirectory(), string.Empty)
            .WithDockerfile("test/OrderMedia.ConsoleApp.IntegrationTests/Dockerfile")
            .WithName("ordermedia/consoleapp/integrationtests:latest")
            .WithCleanUp(true)
            .Build();
        
        // Build image with OrderMedia.ConsoleApp Dockerfile. It doesn't have rights for appsettings.json
        // _dockerImage = new ImageFromDockerfileBuilder()
        //     .WithDockerfileDirectory(CommonDirectoryPath.GetSolutionDirectory(), string.Empty)
        //     .WithDockerfile("src/OrderMedia.ConsoleApp/Dockerfile")
        //     .WithName("ordermedia/consoleapp/integrationtests:latest")
        //     .WithCleanUp(true)
        //     .Build();

        await _dockerImage.CreateAsync();

        _container = new ContainerBuilder()
            .WithImage(_dockerImage)
            .WithEnvironment("MediaSourcePath", "/app/media")
            .WithEnvironment("ImageFolderName", "img")
            .WithEnvironment("VideoFolderName", "vid")
            .WithEnvironment("ImageExtensions:0", ".heic")
            .WithEnvironment("ImageExtensions:1", ".jpg")
            .WithEnvironment("ImageExtensions:2", ".jpeg")
            .WithEnvironment("ImageExtensions:3", ".gif")
            .WithEnvironment("ImageExtensions:4", ".png")
            .WithEnvironment("ImageExtensions:5", ".arw")
            .WithEnvironment("ImageExtensions:6", ".dng")
            .WithEnvironment("ImageExtensions:7", ".insp")
            .WithEnvironment("ImageExtensions:8", ".cr2")
            .WithEnvironment("ImageExtensions:9", ".cr3")
            .WithEnvironment("VideoExtensions:0", ".mov")
            .WithEnvironment("VideoExtensions:1", ".mp4")
            .WithEnvironment("VideoExtensions:2", ".insv")
            .WithEnvironment("OverwriteFiles", "false")
            .WithEnvironment("RenameMediaFiles", "true")
            .WithEnvironment("ReplaceLongNames", "false")
            .WithEnvironment("MaxMediaNameLength", "8")
            .WithEnvironment("NewMediaName", "pbg")
            .WithBindMount(_newMediaPath, "/app/media")
            .Build();
        
        await _container.StartAsync();
        
        Thread.Sleep(TimeSpan.FromSeconds(10));
    }

    [OneTimeTearDown]
    public async Task RemoveInitialSetup()
    {
        await _container.StopAsync();
        await _dockerImage.DeleteAsync();
        Directory.Delete(_newMediaPath, true);
    }
    
    [Test]
    // Raw
    [TestCase("img/2024-05-25/2024-05-25_19-48-49_DSC00397.ARW")] // Arw
    [TestCase("img/2024-05-26/2024-05-26_16-50-17_IMG_3739.DNG")] // Dng
    [TestCase("img/2024-08-03/2024-08-03_19-52-35_IMG_5391.cr2")] // Cr2
    [TestCase("img/2024-07-04/2024-07-04_20-03-26_IMG_0006.cr3")] // Cr3
    // Common photo types
    [TestCase("img/2024-11-24/2024-11-24_14-29-22_IMG_5920.heic")] // Heic
    [TestCase("img/2021-06-05/2021-06-05_20-34-26_IMG_3107.jpeg")] // Jpeg
    [TestCase("img/2021-05-12/2021-05-12_19-03-02_IMG_1749.jpg")] // Jpg
    [TestCase("img/2020-05-07/2020-05-07_18-04-01_IMG_1234.png")] // Png
    [TestCase("img/2024-08-14/2024-08-14_19-38-41_IMG_20240814_193841_00_004.insp")] // Insp
    // Common video types
    [TestCase("vid/2023-05-02/2023-05-02_10-06-42_IMG_4782.MOV")] // Mov
    [TestCase("vid/2022-03-02/2022-03-02_16-43-11_IMG_7043.mp4")] // Mp4
    [TestCase("vid/2024-08-24/LRV_2024-08-24_09-46-46_11_005.insv")] // Insta 360 video
    [TestCase("vid/2024-08-24/VID_2024-08-24_09-46-46_00_005.insv")] // Insta 360 video
    [TestCase("vid/2024-08-24/VID_2024-08-24_09-46-46_10_005.insv")] // Insta 360 video
    // Behaviours
    [TestCase("img/2024-02-03/2024-02-03_14-01-33_DSC09930.ARW")] // RAW with xmp file
    [TestCase("img/2024-02-03/2024-02-03_14-01-33_DSC09930.xmp")] // RAW with xmp file
    [TestCase("img/2024-05-26/2024-05-26_16-03-47_IMG_3635.HEIC")] // LivePhoto
    [TestCase("img/2024-05-26/2024-05-26_16-03-47_IMG_3635.mov")] // LivePhoto
    [TestCase("img/2024-04-09/2024-04-09_19-45-45_PHOTO-2024-04-09-19-45-45.jpg")] // WhatsApp photo
    [TestCase("img/2014-07-31/2014-07-31_22-15-15_GIF-2014-07-31-22-15-15-15.gif")] // WhatsApp Gif
    [TestCase("vid/2024-10-12/2024-10-12_09-34-10_VIDEO-2024-10-12-09-34-10.mp4")] // WhatsApp Video
    [TestCase("img/2024-08-03/2024-08-03_18-29-44_24-08-03 18-29-44 1005.png")] // NextCloud photo
    [TestCase("img/2024-12-11/2024-12-11_20-16-21_IMG_6135.HEIC")] // Photo with aae file
    [TestCase("img/2024-12-11/2024-12-11_20-16-21_IMG_6135.aae")] // Photo with aae file
    // No classify
    [TestCase("no-classify.jpg")] // No classification
    public void GivenMediaFile_WhenExecuted_ClassificationIsPerformedSuccessfully(string mediaPath)
    {
        // Arrange
        var fullMediaPath = Path.Combine(_newMediaPath, mediaPath);
        
        // Act
        
        // Assert
        File.Exists(fullMediaPath).Should().BeTrue();
    }
    
    private static void CopyDirectory(string sourceDir, string destinationDir, bool recursive)
    {
        // Get information about the source directory
        var dir = new DirectoryInfo(sourceDir);

        // Check if the source directory exists
        if (!dir.Exists)
            throw new DirectoryNotFoundException($"Source directory not found: {dir.FullName}");

        // Cache directories before we start copying
        DirectoryInfo[] dirs = dir.GetDirectories();

        // Create the destination directory
        Directory.CreateDirectory(destinationDir);

        // Get the files in the source directory and copy to the destination directory
        foreach (FileInfo file in dir.GetFiles())
        {
            string targetFilePath = Path.Combine(destinationDir, file.Name);
            file.CopyTo(targetFilePath);
        }

        // If recursive and copying subdirectories, recursively call this method
        if (recursive)
        {
            foreach (DirectoryInfo subDir in dirs)
            {
                string newDestinationDir = Path.Combine(destinationDir, subDir.Name);
                CopyDirectory(subDir.FullName, newDestinationDir, true);
            }
        }
    }
}