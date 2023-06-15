namespace OrderMedia.Interfaces
{
    /// <summary>
    /// Configuration service interface.
    /// </summary>
    public interface IConfigurationService
    {
        /// <summary>
        /// Gets the media source path.
        /// </summary>
        /// <returns>String with the media source path.</returns>
        string GetMediaSourcePath();

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