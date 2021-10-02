// <copyright file="DirectoryInfoExtensionsMethods.cs" company="Pablo Bermejo">
// Copyright (c) Pablo Bermejo. All rights reserved.
// </copyright>

namespace OrderMedia.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

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
        public static IEnumerable<FileInfo> GetFilesByExtensions(this DirectoryInfo dir, params string[] extensions)
        {
            if (extensions == null)
            {
                throw new ArgumentNullException("Extensions are needed");
            }

            IEnumerable<FileInfo> files = dir.EnumerateFiles();
            return files.Where(f => extensions.Contains(f.Extension, StringComparer.OrdinalIgnoreCase));
        }
    }
}
