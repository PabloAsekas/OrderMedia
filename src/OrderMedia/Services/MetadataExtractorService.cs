using System;
using System.Collections.Generic;
using MetadataExtractor;
using OrderMedia.Interfaces;

namespace OrderMedia.Services
{
	public class MetadataExtractorService : IMetadataExtractorService
	{
        public IEnumerable<Directory> GetImageDirectories(string mediaPath)
        {
            return ImageMetadataReader.ReadMetadata(mediaPath);
        }
    }
}

