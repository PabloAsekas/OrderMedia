using System.Collections.Generic;

namespace OrderMedia.Configuration;

/// <summary>
/// Represents the Classification settings.
/// </summary>
public class ClassificationSettings
{
    /// <summary>
    /// Gets the configuration section name.
    /// </summary>
    public const string ConfigurationSection = "ClassificationSettings";

    /// <summary>
    /// Gets or sets the classification folders.
    /// </summary>
    public ClassificationFolders Folders { get; init; } = new();

    /// <summary>
    /// Gets or sets the max media name length.
    /// </summary>
    public int MaxMediaNameLength { get; init; }
    
    /// <summary>
    /// Gets or sets the media source path where the media will be classified.
    /// </summary>
    public string MediaSourcePath { get; init; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the new media name.
    /// </summary>
    public string NewMediaName { get; init; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the processor dictionary.
    /// </summary>
    public Dictionary<string, List<string>> Processors { get; init; } = new();
    
    /// <summary>
    /// Gets or sets overwrite files configuration.
    /// </summary>
    public bool OverwriteFiles { get; init; }
    
    /// <summary>
    /// Gets or sets rename media files configuration.
    /// </summary>
    public bool RenameMediaFiles { get; init; }
    
    /// <summary>
    /// Gets or sets replace long names configuration.
    /// </summary>
    public bool ReplaceLongNames { get; init; }
}

/// <summary>
/// Represents the Classification Folders configuration.
/// </summary>
public class ClassificationFolders
{
    /// <summary>
    /// Gets or sets the image folder name.
    /// </summary>
    public string ImageFolderName { get; init; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the video folder name. 
    /// </summary>
    public string VideoFolderName { get; init; } = string.Empty;
}