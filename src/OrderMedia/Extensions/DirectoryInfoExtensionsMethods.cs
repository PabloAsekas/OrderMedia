using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace OrderMedia.Extensions;

/// <summary>
/// <see cref="DirectoryInfo" /> extensions methods class.
/// </summary>
public static class DirectoryInfoExtensionsMethods
{
    /// <summary>
    /// Gets files by extensions.
    /// </summary>
    /// <param name="dir">Directory info.</param>
    /// <param name="extensions">Extensions.</param>
    /// <returns><see cref="IEnumerable{T}"/> with the files that match the given extensions.</returns>
    public static IReadOnlyList<FileInfo> GetFilesByExtensions(this DirectoryInfo dir, params string[] extensions)
    {
        if (extensions == null)
        {
            throw new ArgumentNullException("Extensions are needed");
        }

        IEnumerable<FileInfo> files = dir.EnumerateFiles();
        return files.Where(f => extensions.Contains(f.Extension, StringComparer.OrdinalIgnoreCase)).ToList();
    }

    public static IReadOnlyList<FileInfo> GetAllFilesByExtensions(this DirectoryInfo dir, params string[] extensions)
    {
        if (extensions == null)
        {
            throw new ArgumentNullException("Extensions are needed");
        }

        IEnumerable<FileInfo> files = dir.EnumerateFiles("*", SearchOption.AllDirectories);
        return files.Where(f => extensions.Contains(f.Extension, StringComparer.OrdinalIgnoreCase)).ToList();
    }
}