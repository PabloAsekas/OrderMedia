using OrderMedia.Models;

namespace OrderMedia.Interfaces;

/// <summary>
/// Metadata Extractor Service.
/// </summary>
public interface IMetadataExtractorService
{
	/// <summary>
	/// Gets Created Date for the media path.
	/// </summary>
	/// <param name="mediaPath">Media path.</param>
	/// <returns>Created Date info, if found.</returns>
	CreatedDateInfo? GetCreatedDate(string mediaPath);
}