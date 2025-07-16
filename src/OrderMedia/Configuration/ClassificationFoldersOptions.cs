namespace OrderMedia.Configuration;

/// <summary>
/// Represents the Classification Folders configuration.
/// </summary>
public class ClassificationFoldersOptions
{
    /// <summary>
    /// Gets the configuration section name.
    /// </summary>
    public const string ConfigurationSection = "ClassificationFolders";
    
    /// <summary>
    /// Gets or sets the image folder name.
    /// </summary>
    public required string ImageFolderName { get; init; }
    
    /// <summary>
    /// Gets or sets the video folder name. 
    /// </summary>
    public required string VideoFolderName { get; init; }
}