namespace OrderMedia.Configuration;

public class ClassificationSettingsOptions
{
    public const string ConfigurationSection = "ClassificationSettings";
    
    public int MaxMediaNameLength { get; set; }
    public string NewMediaName { get; set; }
    public bool OverwriteFiles { get; set; }
    public bool RenameMediaFiles { get; set; }
    public bool ReplaceLongNames { get; set; }
}