namespace OrderMedia.Interfaces;

/// <summary>
/// Aae helper service interface.
/// </summary>
public interface IAaeHelperService
{
	/// <summary>
	/// Gets the name of an .aae file based on the original media name without extension.
	/// </summary>
	/// <param name="nameWithoutExtension">Media's name without extension.</param>
	/// <returns>The .aae name, including extension.</returns>
	public string GetAaeName(string nameWithoutExtension);
}