using System;
using System.Collections.Generic;

namespace OrderMedia.Interfaces
{
	/// <summary>
	/// Metadata Extractor Service.
	/// </summary>
	public interface IMetadataExtractorService
	{
		/// <summary>
		/// Gets Image Directory based on media path.
		/// </summary>
		/// <param name="mediaPath">Media path.</param>
		/// <returns>IEnumerable with all the directories of the media.</returns>
		public IEnumerable<MetadataExtractor.Directory> GetImageDirectories(string mediaPath);
    }
}

