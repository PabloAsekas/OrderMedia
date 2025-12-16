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
    /// Process the request from the old media to get the new media.
    /// </summary>
    /// <param name="request">Requests to be processed.</param>
    void Process(ProcessMediaRequest request);
}