namespace OrderMedia.Configuration;

public class ClassificationFoldersOptions
{
    public const string ConfigurationSection = "ClassificationFolders";
    
    public string ImageFolderName { get; set; }
    public string VideoFolderName { get; set; }
}