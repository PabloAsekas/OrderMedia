using System;

namespace OrderMedia.Interfaces;

/// <summary>
/// Created DateTimeOffset extractors interface.
/// </summary>
public interface ICreatedDateExtractorService
{
	/// <summary>
	/// Gets the Created DateTimeOffset of the passed file.
	/// </summary>
	/// <param name="mediaPath">Media full path.</param>
	/// <returns>Media's created DateTimeOffset.</returns>
	public DateTimeOffset GetCreatedDateTimeOffset(string mediaPath);
}