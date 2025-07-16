using System.Collections.Generic;

namespace OrderMedia.Configuration;

/// <summary>
/// Represents the Classification Processors configuration.
/// </summary>
public class ClassificationProcessorsOptions
{
    /// <summary>
    /// Gets the configuration section name.
    /// </summary>
    public const string ConfigurationSection = "ClassificationProcessors";
    
    /// <summary>
    /// Gets or sets the processor dictionary.
    /// </summary>
    public Dictionary<string, List<string>> Processors { get; init; } = new();
}