namespace OrderMedia.Interfaces;

/// <summary>
/// ImageMetadataReader interface.
/// </summary>
public interface IImageMetadataReader
{
    /// <summary>
    /// Gets metadata from the image, searching in the provided directory and tag type.
    /// </summary>
    /// <param name="mediaPath">Media Path.</param>
    /// <param name="tagType">Tag type to search.</param>
    /// <typeparam name="T">Directory to search.</typeparam>
    /// <returns>The tag value, if found.</returns>
    string GetMetadataByDirectoryTypeAndTag<T>(string mediaPath, int tagType);
}