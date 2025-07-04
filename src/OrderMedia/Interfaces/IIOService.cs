using System.Collections.Generic;
using System.IO;

namespace OrderMedia.Interfaces;

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

    /// <summary>
    /// Combines an array of strings into a path.
    /// </summary>
    /// <param name="paths">Path 1.</param>
    /// <returns>The combined paths.</returns>
    string Combine(string[] paths);

    /// <summary>
    /// Returns the extension (including the period ".") of the specified paths string.
    /// </summary>
    /// <param name="path">Path to file.</param>
    /// <returns>The extension of the specified path (including the period "."), null if <paramref name="path"/> is null, or string.Empty if <paramref name="path"/> does not have extension information.</returns>
    string GetExtension(string path);

    /// <summary>
    /// Returns the file name of the specified path string without the extension.
    /// </summary>
    /// <param name="path">Path.</param>
    /// <returns>The name of the file without the period ".".</returns>
    string GetFileNameWithoutExtension(string path);

    /// <summary>
    /// Returns the directory information for the specified path.
    /// </summary>
    /// <param name="path">Path.</param>
    /// <returns>Directory information, null if <paramref name="path"/> denotes a root directory or is null, string.Empty if path does not contain directory information.</returns>
    string GetDirectoryName(string path);

    /// <summary>
    /// Returns the file name and extension of the specified path string.
    /// </summary>
    /// <param name="path">Path.</param>
    /// <returns>The characters afther the last directory separator character in path, null if <paramref name="path"/> is null, or string.Empty if the last character of path is a directory or volume separator character.</returns>
    string GetFileName(string path);

    /// <summary>
    /// Determines whether the specified file exists.
    /// </summary>
    /// <param name="path">Path.</param>
    /// <returns>True if file exists. False otherwise.</returns>
    bool FileExists(string path);
        
    /// <summary>
    /// Determines whether the specified directory exists.
    /// </summary>
    /// <param name="path">Path.</param>
    /// <returns>True if directory exists. False otherwise.</returns>
    bool DirectoryExists(string path);

    /// <summary>
    /// Gets directories inside the provided path.
    /// </summary>
    /// <param name="path">Path.</param>
    /// <returns><see cref="IEnumerable{T}"/> of directories inside the provided path.</returns>
    IEnumerable<string> GetDirectories(string path);

    /// <summary>
    /// Copies an existing file to a new file.
    /// </summary>
    /// <param name="sourceFileName">Source file name.</param>
    /// <param name="destFileName">Destination file name.</param>
    void CopyFile(string sourceFileName, string destFileName);
}