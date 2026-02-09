using OrderMedia.Models;

namespace OrderMedia.ConsoleApp.Interfaces;

public interface IRenamingService
{
    Media Rename(Media original);
}