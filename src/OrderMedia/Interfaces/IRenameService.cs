using System;

namespace OrderMedia.Interfaces
{
    /// <summary>
    /// Rename service interface.
    /// </summary>
    public interface IRenameService
	{
		/// <summary>
		/// Renames given name based on the current name and the created DateTime.
		/// </summary>
		/// <param name="name">Current name.</param>
		/// <param name="createdDateTime">Created DateTime.</param>
		/// <returns>New name.</returns>
		public string Rename(string name, DateTime createdDateTime);

		/// <summary>
		/// Gets the name of an .aae file based on the original media name without extension.
		/// </summary>
		/// <param name="nameWithoutExtension">Media's name without extension.</param>
		/// <returns>The .aae name, including extension.</returns>
		public string GetAaeName(string nameWithoutExtension);
	}
}

