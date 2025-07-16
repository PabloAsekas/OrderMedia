namespace OrderMedia.Configuration;

/// <summary>
/// Represents the Media Extensions configuration.
/// </summary>
public class MediaExtensionsOptions
{
    /// <summary>
    /// Gets the configuration section name.
    /// </summary>
    public const string ConfigurationSection = "MediaExtensions";
    
    /// <summary>
    /// Gets or sets the image extensions.
    /// </summary>
    public required string[] ImageExtensions { get; init; }
    
    /// <summary>
    /// Gets or sets the video extensions.
    /// </summary>
    public required string[] VideoExtensions { get; init; }
}