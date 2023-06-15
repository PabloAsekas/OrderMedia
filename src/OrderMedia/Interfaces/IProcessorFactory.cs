using OrderMedia.Enums;

namespace OrderMedia.Interfaces
{
    /// <summary>
    /// Processor Factory Interface.
    /// </summary>
    public interface IProcessorFactory
	{
		/// <summary>
		/// Creates processor based on the Media Type.
		/// </summary>
		/// <param name="mediaType">Media Type.</param>
		/// <returns>Processor.</returns>
		public IProcessor CreateProcessor(MediaType mediaType);
    }
}

