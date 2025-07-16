using System;
using OrderMedia.Interfaces.Handlers;

namespace OrderMedia.Interfaces.Factories;

/// <summary>
/// Processor Handler Factory Interface.
/// </summary>
public interface IProcessorHandlerFactory
{
	/// <summary>
	/// Creates an instance of the <see cref="IProcessorHandler"/> with the given <see cref="IServiceProvider"/>.
	/// </summary>
	/// <param name="serviceProvider">Service Provider.</param>
	/// <returns>An <see cref="IProcessorHandler"/> instance.</returns>
	IProcessorHandler CreateInstance(IServiceProvider serviceProvider);
}