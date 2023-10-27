namespace OrderMedia.Interfaces
{
	public interface IXmpExtractorService
	{
        /// <summary>
        /// Gets the Created Date from an XMP file.
        /// </summary>
        /// <param name="xmpFilePath">XMP file path.</param>
        /// <returns>Created Date with yyyy-MM-ddTHH:mm:ss format.</returns>
        public string GetCreatedDate(string xmpFilePath);
	}
}

