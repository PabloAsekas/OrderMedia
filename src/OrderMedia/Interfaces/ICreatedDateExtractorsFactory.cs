using OrderMedia.Enums;

namespace OrderMedia.Interfaces
{
    /// <summary>
    /// Created DateTime Service Factory interface.
    /// </summary>
    public interface ICreatedDateExtractorsFactory
	{
        /// <summary>
        /// Gets the CreatedDateExtractors based on the MediaType.
        /// </summary>
        /// <param name="mediaType">Media Type.</param>
        /// <returns>CreatedDateExtractors of the media type.</returns>
        ICreatedDateExtractor GetExtractor(MediaType mediaType);
	}
}

