using OrderMedia.Models;

namespace OrderMedia.ConsoleApp.Interfaces;

/// <summary>
/// Renaming Validator Service.
/// </summary>
public interface IRenamingValidatorService
{
    /// <summary>
    /// Validates if a Media object can be renamed or not.
    /// </summary>
    /// <param name="media">Media object to validate.</param>
    /// <returns>True if Media object can be renamed. False otherwise.</returns>
    bool ValidateMedia(Media media);
}