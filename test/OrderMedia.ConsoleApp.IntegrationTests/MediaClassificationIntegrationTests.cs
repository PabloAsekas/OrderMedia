using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Containers;
using DotNet.Testcontainers.Images;
using FluentAssertions;

namespace OrderMedia.IntegrationTests;

[TestFixture]
public class MediaClassificationIntegrationTests
{
    private readonly string _mediaPath = Path.Combine(TestContext.CurrentContext.TestDirectory, "media");
    private readonly string _newMediaPath = Path.Combine(TestContext.CurrentContext.TestDirectory, "testmedia");
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
            .WithEnvironment("MediaPaths__MediaSourcePath", "/app/media")
            .WithEnvironment("ClassificationFolders__ImageFolderName", "img")
            .WithEnvironment("ClassificationFolders__VideoFolderName", "vid")
            .WithEnvironment("MediaExtensions__ImageExtensions:0", ".heic")
            .WithEnvironment("MediaExtensions__ImageExtensions:1", ".jpg")
            .WithEnvironment("MediaExtensions__ImageExtensions:2", ".jpeg")
            .WithEnvironment("MediaExtensions__ImageExtensions:3", ".gif")
            .WithEnvironment("MediaExtensions__ImageExtensions:4", ".png")
            .WithEnvironment("MediaExtensions__ImageExtensions:5", ".arw")
            .WithEnvironment("MediaExtensions__ImageExtensions:6", ".dng")
            .WithEnvironment("MediaExtensions__ImageExtensions:7", ".insp")
            .WithEnvironment("MediaExtensions__ImageExtensions:8", ".cr2")
            .WithEnvironment("MediaExtensions__ImageExtensions:9", ".cr3")
            .WithEnvironment("MediaExtensions__VideoExtensions:0", ".mov")
            .WithEnvironment("MediaExtensions__VideoExtensions:1", ".mp4")
            .WithEnvironment("MediaExtensions__VideoExtensions:2", ".insv")
            .WithEnvironment("ClassificationSettings__OverwriteFiles", "false")
            .WithEnvironment("ClassificationSettings__RenameMediaFiles", "true")
            .WithEnvironment("ClassificationSettings__ReplaceLongNames", "false")
            .WithEnvironment("ClassificationSettings__MaxMediaNameLength", "8")
            .WithEnvironment("ClassificationSettings__NewMediaName", "pbg")
            .WithBindMount(_newMediaPath, "/app/media")
            .Build();
        
        await _container.StartAsync();
        
        Thread.Sleep(TimeSpan.FromSeconds(20));
    }

    [OneTimeTearDown]
    public async Task RemoveInitialSetup()
    {
        await _container.StopAsync();
        await _dockerImage.DeleteAsync();
        Directory.Delete(_newMediaPath, true);
        await _container.DisposeAsync();
        await _dockerImage.DisposeAsync();
    }
    
    // Raw
    [TestCase("img/2024-03-09/2024-03-09_19-11-36_DSC01943.ARW")] // Arw
    [TestCase("img/2024-05-26/2024-05-26_16-50-17_IMG_3739.DNG")] // Dng
    [TestCase("img/2016-10-06/2016-10-06_18-13-33_IMG_1234.cr2")] // Cr2
    [TestCase("img/2024-07-04/2024-07-04_20-03-26_IMG_0006.cr3")] // Cr3
    // Common photo types
    [TestCase("img/2024-02-03/2024-02-03_20-05-42_IMG_1599.heic")] // Heic
    [TestCase("img/2021-06-05/2021-06-05_20-34-26_IMG_3107.jpeg")] // Jpeg
    [TestCase("img/2021-05-12/2021-05-12_19-03-02_IMG_1749.jpg")] // Jpg
    [TestCase("img/2020-05-07/2020-05-07_18-04-01_IMG_1234.png")] // Png
    [TestCase("img/2024-08-14/2024-08-14_19-38-41_IMG_20240814_193841_00_004.insp")] // Insp - This example is not a real insp file
    // Common video types
    [TestCase("vid/2024-12-27/2024-12-27_14-04-11_IMG_6254.MOV")] // Mov
    [TestCase("vid/1904-01-01/1904-01-01_00-00-00_IMG_7354.mp4")] // Mp4
    [TestCase("vid/2024-08-24/LRV_2024-08-24_09-46-46_11_005.insv")] // Insta 360 video - This example is not a real insv file
    [TestCase("vid/2024-08-24/VID_2024-08-24_09-46-46_00_005.insv")] // Insta 360 video - This example is not a real insv file
    [TestCase("vid/2024-08-24/VID_2024-08-24_09-46-46_10_005.insv")] // Insta 360 video - This example is not a real insv file
    [TestCase("vid/2024-08-24/VID_2024-08-24_09-46-46_10_005.insv")] // Insta 360 video - This example is not a real insv file
    // Behaviours
    [TestCase("img/2024-05-17/2024-05-17_10-11-12_DSC01944.ARW")] // RAW with xmp file
    [TestCase("img/2024-05-17/2024-05-17_10-11-12_DSC01944.xmp")] // RAW with xmp file
    [TestCase("img/2024-12-27/2024-12-27_13-58-31_IMG_6252.HEIC")] // LivePhoto
    [TestCase("img/2024-12-27/2024-12-27_13-58-31_IMG_6252.mov")] // LivePhoto
    [TestCase("img/2024-04-09/2024-04-09_19-45-45_PHOTO-2024-04-09-19-45-45.jpg")] // WhatsApp photo
    [TestCase("img/2014-07-31/2014-07-31_22-15-15_GIF-2014-07-31-22-15-15-15.gif")] // WhatsApp Gif
    [TestCase("vid/2024-10-12/2024-10-12_09-34-10_VIDEO-2024-10-12-09-34-10.mp4")] // WhatsApp Video
    [TestCase("img/2024-08-03/2024-08-03_18-29-44_24-08-03 18-29-44 1005.png")] // NextCloud photo
    [TestCase("img/2024-12-27/2024-12-27_14-06-29_IMG_6253.HEIC")] // Photo with aae file
    [TestCase("img/2024-12-27/2024-12-27_14-06-29_IMG_6253.aae")] // Photo with aae file
    [TestCase("vid/2025-02-22/2025-02-22_11-35-21_C0001.mp4")] //Mp4 with xml file
    [TestCase("vid/2025-02-22/2025-02-22_11-35-21_C0001M01.XML")] //Mp4 with xml file
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