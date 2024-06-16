using OrderMedia.Models;

namespace OrderMedia.Interfaces.Handlers;

/// <summary>
/// Processor Handler interface.
/// </summary>
public interface IProcessorHandler
{
    /// <summary>
    /// Sets next handler to be executed.
    /// </summary>
    /// <param name="handler">Handler to be executed.</param>
    /// <returns>Handler to be executed.</returns>
    IProcessorHandler SetNext(IProcessorHandler handler);
    
    /// <summary>
    /// Process the media.
    /// </summary>
    /// <param name="media">Media to be processed.</param>
    void Process(Media media);
}