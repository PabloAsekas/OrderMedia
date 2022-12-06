using OrderMedia.MediaFiles;

namespace OrderMedia.Interfaces
{
    /// <summary>
    /// Media factory service interface.
    /// </summary>
    public interface IMediaFactoryService
    {
        /// <summary>
        /// Creates media object based on the path of the file.
        /// </summary>
        /// <param name="path">Full path.</param>
        /// <returns>Media object.</returns>
        BaseMedia CreateMedia(string path);
    }
}
