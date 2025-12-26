using OrderMedia.Models;

namespace OrderMedia.ConsoleApp.Interfaces;

/// <summary>
/// Classification Service 
/// </summary>
public interface IClassificationService
{
    /// <summary>
    /// Creates a Media object with the new parameters after the classification.
    /// </summary>
    /// <param name="original">Original Media to be classified.</param>
    /// <returns>A new Media object.</returns>
    Media Classify(Media original);
}