using OrderMedia.ConsoleApp.Interfaces;

namespace OrderMedia.ConsoleApp.Services;

public class SyncExclusionService : ISyncExclusionService
{
    public IEnumerable<string> Exclude(IEnumerable<string> source, IEnumerable<string> exclude)
    {
        return source.Except(exclude);
    }
}