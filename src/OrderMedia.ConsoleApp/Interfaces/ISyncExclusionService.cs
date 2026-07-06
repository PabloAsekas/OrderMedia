namespace OrderMedia.ConsoleApp.Interfaces;

public interface ISyncExclusionService
{
    IEnumerable<string> Exclude(IEnumerable<string> source, IEnumerable<string> exclude);
}