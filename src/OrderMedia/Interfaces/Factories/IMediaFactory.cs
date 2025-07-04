using OrderMedia.Models;

namespace OrderMedia.Interfaces.Factories;

/// <summary>
/// Media factory service interface.
/// </summary>
public interface IMediaFactory
{
    /// <summary>
    /// Creates media object based on the path of the file.
    /// </summary>
    /// <param name="path">Full path.</param>
    /// <returns>Media object.</returns>
    Media CreateMedia(string path);
}