namespace OrderMedia.Configuration;

/// <summary>
/// Represents the Media Paths configuration.
/// </summary>
public class MediaPathsOptions
{
    /// <summary>
    /// Gets the configuration section name.
    /// </summary>
    public const string ConfigurationSection = "MediaPaths";
    
    /// <summary>
    /// Gets or sets the media post-process path.
    /// </summary>
    public required string MediaPostProcessPath { get; init; }

    /// <summary>
    /// Gets or sets the media post-process source.
    /// </summary>
    public required string MediaPostProcessSource { get; init; }
    
    /// <summary>
    /// Gets or sets the media source path.
    /// </summary>
    public required string MediaSourcePath { get; init; }
}