using OrderMedia.Enums;

namespace OrderMedia.ConsoleApp.Interfaces;

/// <summary>
/// Classification Media Folder Strategy Resolver.
/// </summary>
public interface IClassificationMediaFolderStrategyResolver
{
    /// <summary>
    /// Returns the strategy that can handle the given media type.
    /// </summary>
    /// <param name="mediaType">Media Type.</param>
    /// <returns>The strategy that can handle the given media type.</returns>
    IClassificationMediaFolderStrategy Resolve(MediaType mediaType);
}