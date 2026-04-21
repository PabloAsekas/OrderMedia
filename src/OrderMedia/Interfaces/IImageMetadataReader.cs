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

    /// <summary>
    /// Gets metadata from the image, searching in the <see cref="XmpDirectory"/>XmpDirectory with the provided property name.
    /// </summary>
    /// <param name="mediaPath">Media Path</param>
    /// <param name="propertyName">Name of the XMP property.</param>
    /// <returns>The property value, if found.</returns>
    string GetMetadataFromXmpDirectory(string mediaPath, string propertyName);
}