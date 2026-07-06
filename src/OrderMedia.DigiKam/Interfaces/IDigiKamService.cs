using OrderMedia.DigiKam.Models;

namespace OrderMedia.DigiKam.Interfaces;

/// <summary>
/// DigiKam Service
/// </summary>
public interface IDigiKamService
{
    /// <summary>
    /// Get media paths by the desired request.
    /// </summary>
    /// <param name="request"><see cref="GetMediaPathsRequest"/> object including all the details of the request.</param>
    /// <param name="cancellationToken">Cancellation Token.</param>
    /// <returns>String list with the paths matching the request details. Paths are in alphabetical order.</returns>
    Task<IEnumerable<string>> GetMediaRelativePathsAsync(GetMediaPathsRequest request, CancellationToken cancellationToken = default);
}