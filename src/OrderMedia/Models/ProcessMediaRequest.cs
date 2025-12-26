namespace OrderMedia.Models;

public class ProcessMediaRequest
{
    /// <summary>
    /// <see cref="Media"/> objet representing the original media to be processed.
    /// </summary>
    public Media Original { get; init; } = new();
    
    /// <summary>
    /// <see cref="Media"/> objetc representing the media result after being processed.
    /// </summary>
    public Media Target { get; init; } = new();
    
    /// <summary>
    /// Overwrite files if the file already exists.
    /// </summary>
    public bool OverwriteFiles { get; init; }
}