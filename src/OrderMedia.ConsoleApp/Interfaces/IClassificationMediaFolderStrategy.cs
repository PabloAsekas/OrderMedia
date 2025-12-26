using OrderMedia.Enums;

namespace OrderMedia.ConsoleApp.Interfaces;

/// <summary>
/// Classification media folder strategy interface.
/// </summary>
public interface IClassificationMediaFolderStrategy
{
    /// <summary>
    /// Whether the strategy can handle the given media type or not.
    /// </summary>
    /// <param name="mediaType">Media Type to handle.</param>
    /// <returns>True if strategy can handle the given media type. False otherwise.</returns>
    bool CanHandle(MediaType mediaType);
    
    /// <summary>
    /// Gets the target folder configured for the strategy.
    /// </summary>
    /// <returns>The name of the folder.</returns>
    string GetTargetFolder();
}