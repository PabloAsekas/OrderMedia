using OrderMedia.Enums;

namespace OrderMedia.Interfaces.Factories;

/// <summary>
/// Rename Strategy Factory Interface.
/// </summary>
public interface IRenameStrategyFactory
{
    /// <summary>
    /// Creates Rename Strategy based on the Media Type.
    /// </summary>
    /// <param name="mediaType">Media Type.</param>
    /// <returns>Rename Strategy.</returns>
    IRenameStrategy GetRenameStrategy(MediaType mediaType);
}