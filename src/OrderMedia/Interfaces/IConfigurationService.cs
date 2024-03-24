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

        /// <summary>
        /// Gets the setting to overwrite files or not.
        /// </summary>
        /// <returns>Bool with the overwrite files setting.</returns>
        bool GetOverwriteFiles();

        /// <summary>
        /// Gets the setting to rename media files or not.
        /// </summary>
        /// <returns>Bool with the rename meda file setting.</returns>
        bool GetRenameMediaFiles();

        /// <summary>
        /// Gets the setting to replace long names with new ones.
        /// </summary>
        /// <returns>Bool with the replace long names setting.</returns>
        bool GetReplaceLongNames();

        /// <summary>
        /// Gets the maximum length setting for media names to be replaced with new name.
        /// </summary>
        /// <returns>Integer with the maximum media name length.</returns>
        int GetMaxMediaNameLength();

        /// <summary>
        /// Gets the new media name if the original media name will be replaced.
        /// </summary>
        /// <returns>String with the new media name.</returns>
        string GetNewMediaName();

        /// <summary>
        /// Gets the media post process path.
        /// </summary>
        /// <returns>String with the media export path.</returns>
        string GetMediaPostProcessPath();

        /// <summary>
        /// Gets the media post process source path.
        /// </summary>
        /// <returns>String with the media export path.</returns>
        string GetMediaPostProcessSource();

    }
}