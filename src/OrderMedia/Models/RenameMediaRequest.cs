using System;

namespace OrderMedia.Models;

public class RenameMediaRequest
{
    /// <summary>
    /// Media's name, with extension.
    /// </summary>
    public string Name { get; init; } = string.Empty;
    
    /// <summary>
    /// Media's created date.
    /// </summary>
    public DateTimeOffset CreatedDate { get; init; }
    
    /// <summary>
    /// Replace name if the maximum name length is exceeded.
    /// </summary>
    public bool ReplaceName { get; init; }
    
    /// <summary>
    /// Defines the maximum name length. Names with higher length than this will be replaced.
    /// </summary>
    public int MaximumNameLength { get; init; }
    
    /// <summary>
    /// The new name that will be applied if there is a name replace.
    /// </summary>
    public string NewName { get; init; } = string.Empty;
    
}