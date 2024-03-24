namespace OrderMedia.Interfaces;

public interface ICopyComplementFilesService
{
    void CopyComplementFiles(string filesToApply, string extensionToSearch);
}