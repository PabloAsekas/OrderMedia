namespace OrderMedia.Configuration;

/// <summary>
/// Represents the Classification settings.
/// </summary>
public class ClassificationSettingsOptions
{
    /// <summary>
    /// Gets the configuration section name.
    /// </summary>
    public const string ConfigurationSection = "ClassificationSettings";
    
    /// <summary>
    /// Gets or sets the max media name length.
    /// </summary>
    public required int MaxMediaNameLength { get; init; }
    
    /// <summary>
    /// Gets or sets the new media name.
    /// </summary>
    public required string NewMediaName { get; init; }
    
    /// <summary>
    /// Gets or sets overwrite files configuration.
    /// </summary>
    public required bool OverwriteFiles { get; init; }
    
    /// <summary>
    /// Gets or sets rename media files configuration.
    /// </summary>
    public required bool RenameMediaFiles { get; init; }
    
    /// <summary>
    /// Gets or sets replace long names configuration.
    /// </summary>
    public required bool ReplaceLongNames { get; init; }
}