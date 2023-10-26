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

        /// <summary>
        /// Gets Created Date for Raw images.
        /// </summary>
        /// <param name="mediaPath">Raw media path.</param>
        /// <returns>Created date with yyyy:MM:dd HH:mm:ss format.</returns>
        public string GetRawCreatedDate(string mediaPath);

        /// <summary>
        /// Gets Created Date for videos.
        /// </summary>
        /// <param name="mediaPath">Video media path.</param>
        /// <returns>Created date with ddd MMM dd HH:mm:ss zzz yyyy format.</returns>
        public string GetVideoCreatedDate(string mediaPath);

        /// <summary>
        /// Gets Created Date for regular images like .jpg, .jpge, .png, .heic and .gif.
        /// </summary>
        /// <param name="mediaPath">Image media path.</param>
        /// <returns>Created date with yyyy:MM:dd HH:mm:ss format.</returns>
        public string GetImageCreatedDate(string mediaPath);
    }
}
