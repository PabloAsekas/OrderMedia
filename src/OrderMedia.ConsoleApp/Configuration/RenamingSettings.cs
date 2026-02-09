namespace OrderMedia.ConsoleApp.Configuration;

public class RenamingSettings
{
    public const string ConfigurationSection = "RenamingSettings";
    public int MaxMediaNameLength { get; set; }
    public string MediaSourcePath { get; set; } = string.Empty;
    public string NewMediaName { get; set; } = string.Empty;
    public bool ReplaceLongNames { get; set; }
}