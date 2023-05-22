namespace OrderMedia.Interfaces
{
    /// <summary>
    /// Configuration service interface.
    /// </summary>
    public interface IConfigurationService
    {
        /// <summary>
        /// Gets the original path.
        /// </summary>
        /// <returns>String with the original path.</returns>
        string GetOriginalPath();

        /// <summary>
        /// Gets image extensions.
        /// </summary>
        /// <returns>String array with the image extensions.</returns>
        string[] GetImageExtensions();

        /// <summary>
        /// Gets video extensions.
        /// </summary>
        /// <returns>String array with the video extensions.</returns>
        string[] GetVideoExtensions();

        /// <summary>
        /// Gets image folder name.
        /// </summary>
        /// <returns>String with the image folder name.</returns>
        string GetImageFolderName();

        /// <summary>
        /// Gets video folder name.
        /// </summary>
        /// <returns>String with the video folder name.</returns>
        string GetVideoFolderName();
    }
}