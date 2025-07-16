using OrderMedia.Enums;
using OrderMedia.Interfaces.Handlers;

namespace OrderMedia.Interfaces.Factories;

/// <summary>
/// Processor chain factory interface.
/// </summary>
public interface IProcessorChainFactory
{
    /// <summary>
    /// Builds the processor chain based on the MediaType.
    /// </summary>
    /// <param name="key"><see cref="MediaType"/> that will be used to build the chain.</param>
    /// <returns>A chain in form of single <see cref="IProcessorHandler"/> with all the handlers configured.</returns>
    IProcessorHandler? Build(MediaType key);
}