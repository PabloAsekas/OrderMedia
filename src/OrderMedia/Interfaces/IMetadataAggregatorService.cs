using SixLabors.ImageSharp;

namespace OrderMedia.Interfaces;

/// <summary>
/// Metadata Aggregator Service Interface.
/// </summary>
public interface IMetadataAggregatorService
{
    /// <summary>
    /// Get the <see cref="Image"/> object of the provided image path.
    /// </summary>
    /// <param name="imagePath">Image path.</param>
    /// <returns><see cref="Image"/> object of the provided image path.</returns>
    public Image GetImage(string imagePath);

    /// <summary>
    /// Saves the <see cref="Image"/> object.
    /// </summary>
    /// <param name="image">Image to save.</param>
    /// <param name="path">Path to save the file.</param>
    public void SaveImage(Image image, string path);
}