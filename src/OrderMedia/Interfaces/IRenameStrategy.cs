using System;

namespace OrderMedia.Interfaces;

public interface IRenameStrategy
{
    /// <summary>
    /// Renames given name based on the current name and the created DateTime.
    /// </summary>
    /// <param name="name">Current name.</param>
    /// <param name="createdDateTime">Created DateTime.</param>
    /// <returns>New name.</returns>
    public string Rename(string name, DateTime createdDateTime);
}