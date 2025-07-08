namespace OrderMedia.Configuration;

public class MediaExtensionsOptions
{
    public const string ConfigurationSection = "MediaExtensions";
    
    public string[] ImageExtensions { get; set; }
    public string[] VideoExtensions { get; set; }
}