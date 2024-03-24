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
        private readonly IConfigurationService _configurationService;

        public IOService(IConfigurationService configurationService)
        {
            _configurationService = configurationService;
        }

        public IEnumerable<FileInfo> GetFilesByExtensions(string path, params string[] extensions)
        {
            var directory = new DirectoryInfo(path);
            return directory.GetFilesByExtensions(extensions);
        }

        public void MoveMedia(string oldPath, string newPath)
        {
            File.Move(oldPath, newPath, _configurationService.GetOverwriteFiles());
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

        public bool FileExists(string path)
        {
            return File.Exists(path);
        }
        
        public bool DirectoryExists(string path)
        {
            return Directory.Exists(path);
        }

        public IEnumerable<string> GetDirectories(string path)
        {
            return Directory.GetDirectories(path);
        }

        public void CopyFile(string sourceFileName, string destFileName)
        {
            File.Copy(sourceFileName, destFileName);
        }
    }
}
