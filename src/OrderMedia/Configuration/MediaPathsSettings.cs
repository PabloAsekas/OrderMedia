namespace OrderMedia.Configuration;

/// <summary>
/// Represents the Media Paths configuration.
/// </summary>
public class MediaPathsSettings
{
    /// <summary>
    /// Gets the configuration section name.
    /// </summary>
    public const string ConfigurationSection = "MediaPaths";
    
    /// <summary>
    /// Gets or sets the media post-process path.
    /// </summary>
    public string MediaPostProcessPath { get; init; } = string.Empty;

    /// <summary>
    /// Gets or sets the media post-process source.
    /// </summary>
    public string MediaPostProcessSource { get; init; } = string.Empty;
}