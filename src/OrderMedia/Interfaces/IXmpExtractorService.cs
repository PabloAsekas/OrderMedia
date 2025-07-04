namespace OrderMedia.Interfaces;

/// <summary>
/// XML Extractor Service Interface.
/// </summary>
public interface IXmpExtractorService
{
	/// <summary>
	/// Gets the Created Date from an XML file.
	/// </summary>
	/// <param name="xmlFilePath">XML file path.</param>
	/// <returns>Created date.</returns>
	public string GetCreatedDate(string xmlFilePath);
}