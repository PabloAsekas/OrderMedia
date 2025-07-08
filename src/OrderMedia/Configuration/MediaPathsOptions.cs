namespace OrderMedia.Configuration;

public class MediaPathsOptions
{
    public const string ConfigurationSection = "MediaPaths";
    
    public string MediaPostProcessPath { get; set; }
    public string MediaPostProcessSource { get; set; }
    public string MediaSourcePath { get; set; }
}