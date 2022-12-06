using System.Collections.Generic;
using System.IO;
using OrderMedia.MediaFiles;

namespace OrderMedia.Interfaces
{
    /// <summary>
    /// IIOService interface.
    /// </summary>
    public interface IIOService
    {
        /// <summary>
        /// Gets files by the given extensions.
        /// </summary>
        /// <param name="extensions">Extensions array.</param>
        /// <returns><see cref="IEnumerable{T}"/> of files that match the given extensions.</returns>
        IEnumerable<FileInfo> GetFilesByExtensions(string path, params string[] extensions);

        /// <summary>
        /// Moves media to the desire sub folder.
        /// </summary>
        /// <param name="file">File to be moved.</param>
        /// <param name="subFolderName">Name of the sub folder.</param>
        void MoveMedia(string oldPath, string newPath);

        /// <summary>
        /// Creates the passed folder.
        /// </summary>
        /// <param name="path">Path of the new folder to be created.</param>
        void CreateFolder(string path);
    }
}