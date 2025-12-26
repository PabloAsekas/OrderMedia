namespace OrderMedia.ConsoleApp.Configuration;

/// <summary>
/// Represents the Media Extensions configuration.
/// </summary>
public class MediaExtensionsSettings
{
    /// <summary>
    /// Gets the configuration section name.
    /// </summary>
    public const string ConfigurationSection = "MediaExtensions";

    /// <summary>
    /// Gets or sets the image extensions.
    /// </summary>
    public string[] ImageExtensions { get; init; } = [];

    /// <summary>
    /// Gets or sets the video extensions.
    /// </summary>
    public string[] VideoExtensions { get; init; } = [];
}