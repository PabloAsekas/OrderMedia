using OrderMedia.Models;

namespace OrderMedia.Interfaces;

public interface IRenameStrategy
{
    /// <summary>
    /// Renames given name based on the current name and the created DateTimeOffset.
    /// </summary>
    /// <param name="request">Rename Media Request.</param>
    /// <returns>New name.</returns>
    // public string Rename(string name, DateTimeOffset createdDateTimeOffset);
    public string Rename(RenameMediaRequest request);
}