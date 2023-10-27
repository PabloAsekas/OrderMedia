using Microsoft.Extensions.Configuration;
using OrderMedia.Interfaces;

namespace OrderMedia.Services
{
    /// <summary>
    /// Configuration service class.
    /// </summary>
    public class ConfigurationService : IConfigurationService
    {
        private readonly IConfiguration _configuration;

        public ConfigurationService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GetMediaSourcePath()
        {
            var section = _configuration.GetSection("MediaSourcePath");
            return section.Get<string>();
        }

        public string[] GetImageExtensions()
        {
            var section = _configuration.GetSection("ImageExtensions");
            return section.Get<string[]>();
        }

        public string[] GetVideoExtensions()
        {
            var section = _configuration.GetSection("VideoExtensions");
            return section.Get<string[]>();
        }

        public string GetImageFolderName()
        {
            var section = _configuration.GetSection("ImageFolderName");
            return section.Get<string>();
        }

        public string GetVideoFolderName()
        {
            var section = _configuration.GetSection("VideoFolderName");
            return section.Get<string>();
        }

        public bool GetOverwriteFiles()
        {
            var section = _configuration.GetSection("OverwriteFiles");
            return section.Get<bool>();
        }

        public bool GetRenameMediaFiles()
        {
            var section = _configuration.GetSection("RenameMediaFiles");
            return section.Get<bool>();
        }

        public bool GetReplaceLongNames()
        {
            var section = _configuration.GetSection("ReplaceLongNames");
            return section.Get<bool>();
        }

        public int GetMaxMediaNameLength()
        {
            var section = _configuration.GetSection("MaxMediaNameLength");
            return section.Get<int>();
        }

        public string GetNewMediaName()
        {
            var section = _configuration.GetSection("NewMediaName");
            return section.Get<string>();
        }
    }
}
