using OrderMedia.Models;

namespace OrderMedia.Interfaces.Handlers;

/// <summary>
/// Created Date Handler interface.
/// </summary>
public interface ICreatedDateHandler
{
    /// <summary>
    /// Sets next handler to be executed.
    /// </summary>
    /// <param name="handler">Handler to be executed.</param>
    /// <returns>Handler to be executed.</returns>
    ICreatedDateHandler SetNext(ICreatedDateHandler handler);
    
    /// <summary>
    /// Gets CreatedDate parameter by the given mediaPath.
    /// </summary>
    /// <param name="mediaPath">Media Path.</param>
    /// <returns>CreatedDate info, if found.</returns>
    CreatedDateInfo GetCreatedDateInfo(string mediaPath);
}