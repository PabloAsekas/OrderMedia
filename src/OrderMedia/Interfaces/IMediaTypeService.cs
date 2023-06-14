using System;
using OrderMedia.Enums;

namespace OrderMedia.Interfaces
{
	/// <summary>
	/// Media Type Service interface.
	/// </summary>
	public interface IMediaTypeService
	{
		/// <summary>
		/// Gets media type based on the media path.
		/// </summary>
		/// <param name="path">Full path.</param>
		/// <returns>The MediaType of the passed path.</returns>
		MediaType GetMediaType(string path);
	}
}

