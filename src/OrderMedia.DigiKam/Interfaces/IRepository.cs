using OrderMedia.DigiKam.Models;

namespace OrderMedia.DigiKam.Interfaces;

/// <summary>
/// DigiKam's Repository Interface.
/// </summary>
public interface IRepository
{
    /// <summary>
    /// Gets tag by tag name.
    /// </summary>
    /// <param name="tagName">Tag Name.</param>
    /// <param name="cancellationToken">Cancellation Token.</param>
    /// <returns>A <see cref="Tag"/> if tag is found by tag name. Null otherwise.</returns>
    Task<Tag?> GetTagByTagNameAsync(string tagName, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Gets media paths by the desire query.
    /// </summary>
    /// <param name="query"><see cref="GetMediaPathsQuery"/> object with all the query details.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns><see cref="IEnumerable{string}"/> with the paths found matching the desired query. Empty collection if nothing found.</returns>
    Task<IEnumerable<string>> GetMediaPathsAsync(GetMediaPathsQuery query, CancellationToken cancellationToken = default);
}