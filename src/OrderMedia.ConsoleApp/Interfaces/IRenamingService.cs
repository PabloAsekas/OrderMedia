using OrderMedia.Models;

namespace OrderMedia.ConsoleApp.Interfaces;

/// <summary>
/// Renaming Service
/// </summary>
public interface IRenamingService
{
    /// <summary>
    /// Creates a Media object with the new parameters after the renaming.
    /// </summary>
    /// <param name="original">Original to be renamed.</param>
    /// <returns>A new Media object.</returns>
    Media Rename(Media original);
}