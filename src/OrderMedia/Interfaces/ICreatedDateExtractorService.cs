using System;

namespace OrderMedia.Interfaces
{
    /// <summary>
    /// Created DateTime extractors interface.
    /// </summary>
    public interface ICreatedDateExtractorService
	{
		/// <summary>
		/// Gets the Created DateTime of the passed file.
		/// </summary>
		/// <param name="mediaPath">Media full path.</param>
		/// <returns>Media's created DateTime.</returns>
		public DateTime GetCreatedDateTime(string mediaPath);
	}
}

