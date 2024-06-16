using OrderMedia.Enums;
using OrderMedia.Interfaces.Handlers;

namespace OrderMedia.Interfaces.Factories
{
    /// <summary>
    /// Processor Handler Factory Interface.
    /// </summary>
    public interface IProcessorHandlerFactory
	{
		/// <summary>
		/// Creates processor handler based on the Media Type.
		/// </summary>
		/// <param name="mediaType">Media Type.</param>
		/// <returns>Processor Handler.</returns>
		public IProcessorHandler CreateProcessorHandler(MediaType mediaType);
    }
}

