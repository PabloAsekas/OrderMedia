using OrderMedia.Models;

namespace OrderMedia.ConsoleApp.Interfaces;

public interface IRenamingValidatorService
{
    bool ValidateMedia(Media media);
}