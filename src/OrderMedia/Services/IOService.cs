using System.Collections.Generic;
using System.IO;
using OrderMedia.Extensions;
using OrderMedia.Interfaces;

namespace OrderMedia.Services
{
    /// <summary>
    /// IO service class.
    /// </summary>
    public class IOService : IIOService
    {
        public IEnumerable<FileInfo> GetFilesByExtensions(string path, params string[] extensions)
        {
            var directory = new DirectoryInfo(path);
            return directory.GetFilesByExtensions(extensions);
        }

        public void MoveMedia(string oldPath, string newPath)
        {
            File.Move(oldPath, newPath);
        }

        public void CreateFolder(string path)
        {
            Directory.CreateDirectory(path);
        }

        public string Combine(string[] paths)
        {
            return Path.Combine(paths);
        }

        public string GetExtension(string path)
        {
            return Path.GetExtension(path);
        }

        public string GetFileNameWithoutExtension(string path)
        {
            return Path.GetFileNameWithoutExtension(path);
        }

        public string GetDirectoryName(string path)
        {
            return Path.GetDirectoryName(path);
        }

        public string GetFileName(string path)
        {
            return Path.GetFileName(path);
        }

        public bool Exists(string path)
        {
            return File.Exists(path);
        }
    }
}
