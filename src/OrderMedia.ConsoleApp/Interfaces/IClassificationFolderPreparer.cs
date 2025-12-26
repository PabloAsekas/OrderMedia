namespace OrderMedia.ConsoleApp.Interfaces;

/// <summary>
/// Classification Folder Preparer Interface.
/// </summary>
public interface IClassificationFolderPreparer
{
    /// <summary>
    /// Creates the needed folders to perform the classification in case they don't exist.
    /// </summary>
    void Prepare();
}