using OrderMedia.ConsoleApp.Interfaces;
using OrderMedia.Enums;

namespace OrderMedia.ConsoleApp.Strategies.FolderStrategy;

public class ClassificationMediaFolderStrategyResolver : IClassificationMediaFolderStrategyResolver
{
    private readonly IReadOnlyCollection<IClassificationMediaFolderStrategy> _strategies;

    public ClassificationMediaFolderStrategyResolver(IEnumerable<IClassificationMediaFolderStrategy> strategies)
    {
        _strategies = strategies.ToList();
    }

    public IClassificationMediaFolderStrategy Resolve(MediaType mediaType)
    {
        var strategy = _strategies.FirstOrDefault(s => s.CanHandle(mediaType));

        return strategy ?? throw new NotSupportedException($"No strategy registered to support media type '{mediaType}'.");
    }
}