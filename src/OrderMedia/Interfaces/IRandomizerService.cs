namespace OrderMedia.Interfaces;

/// <summary>
/// Randomizer Service Interface.
/// </summary>
public interface IRandomizerService
{
	/// <summary>
	/// Gets a random number between 0 and 9999 as D4 format: 0001, 1343, 3231.
	/// </summary>
	/// <returns>String with the random number as D4 format.</returns>
	public string GetRandomNumberAsD4();
}