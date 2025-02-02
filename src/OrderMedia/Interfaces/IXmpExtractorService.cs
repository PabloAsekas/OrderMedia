namespace OrderMedia.Interfaces
{
	/// <summary>
	/// XML Extractor Service Interface.
	/// </summary>
	public interface IXmpExtractorService
	{
        /// <summary>
        /// Gets the value from an XML file given by its schema name and its property name.
        /// </summary>
        /// <param name="xmlFilePath">XML file path.</param>
        /// <param name="schemaName">Schema name.</param>
        /// <param name="propertyName">Property name.</param>
        /// <returns>The value.</returns>
        public string GetValue(string xmlFilePath, string schemaName, string propertyName);
	}
}

