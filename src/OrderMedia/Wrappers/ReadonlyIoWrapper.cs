using System;
using System.Collections.Generic;
using System.IO;
using OrderMedia.Extensions;
using OrderMedia.Interfaces;

namespace OrderMedia.Wrappers;

public class ReadonlyIoWrapper : IIoWrapper, IDisposable
{
    private readonly StreamWriter _file;

    public ReadonlyIoWrapper()
    {
        _file = new StreamWriter("/Users/pablo/Downloads/file.txt", true);
    }
    
    public IEnumerable<FileInfo> GetFilesByExtensions(string path, params string[] extensions)
    {
        var directory = new DirectoryInfo(path);
        return directory.GetFilesByExtensions(extensions);
    }
    
    public IEnumerable<FileInfo> GetAllFilesByExtensions(string path, params string[] extensions)
    {
        var directory = new DirectoryInfo(path);
        return directory.GetAllFilesByExtensions(extensions);
    }

    public void MoveMedia(string oldPath, string newPath, bool overwrite)
    {
        _file.WriteLine($"Moving '{oldPath}' to '{newPath}'");
    }

    public void CreateFolder(string path)
    {
        _file.WriteLine($"Creating folder '{path}'");
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
        return Path.GetDirectoryName(path)!;
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
        _file.WriteLine($"Copying '{sourceFileName}' to '{destFileName}'");
    }

    public void RejectMedia(string path)
    {
        _file.WriteLine($"Rejecting '{path}'");
    }

    public void Dispose()
    {
        _file.Dispose();
    }
}