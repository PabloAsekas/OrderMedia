using OrderMedia.Models;

namespace OrderMedia.Interfaces
{
    /// <summary>
    /// Processor interface.
    /// </summary>
    public interface IProcessor
	{
		/// <summary>
		/// Executes the processor method for the desired media.
		/// </summary>
		/// <param name="media">Media to be processed.</param>
		public void Execute(Media media);

		/// <summary>
		/// Sets the processor.
		/// </summary>
		/// <param name="processor">Processor to be set.</param>
		public void SetProcessor(IProcessor processor);
    }
}

