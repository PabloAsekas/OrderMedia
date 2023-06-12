using System;
using OrderMedia.Models;

namespace OrderMedia.Interfaces
{
	/// <summary>
	/// Classification Service interface.
	/// </summary>
	public interface IClassificationService
	{
		/// <summary>
		/// Process the passed media.
		/// </summary>
		/// <param name="media">Media.</param>
		public void Process(Media media);
	}
}

