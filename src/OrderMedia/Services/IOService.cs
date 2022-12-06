using System.Collections.Generic;
using System.IO;
using OrderMedia.Extensions;
using OrderMedia.Interfaces;
using OrderMedia.MediaFiles;

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
    }
}
