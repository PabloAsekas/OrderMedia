namespace OrderMedia.ConsoleApp.Configuration;

/// <summary>
/// Represents the Synchronization settings.
/// </summary>
public class SynchronizationSettings
{
    /// <summary>
    /// Gets the configuration section name.
    /// </summary>
    public const string ConfigurationSection = "SynchronizationSettings";
    
    public string DestinationPath { get; set; }
    
    public DigikamOptions DigiKam { get; set; }
    
    public ImmichOptions Immich { get; set; }
    
    public List<RequestRule> RequestRules { get; set; }
    
    public string SourcePath { get; set; }
}

public sealed class RequestRule
{
    public string[] ExcludingTags { get; set; }
    public string Format { get; set; }
    public string[] IncludingTags { get; set; }
}

public sealed class DigikamOptions
{
    public string ConnectionString { get; set; }
}

public sealed class ImmichOptions
{
    public string Url { get; set; }
    public string ApiKey { get; set; }
}