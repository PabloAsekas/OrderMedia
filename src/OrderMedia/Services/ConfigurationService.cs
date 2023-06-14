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

        public string GetOriginalPath()
        {
            var section = _configuration.GetSection("originalPath");
            return section.Get<string>();
        }

        public string[] GetImageExtensions()
        {
            var section = _configuration.GetSection("imageExtensions");
            return section.Get<string[]>();
        }

        public string[] GetVideoExtensions()
        {
            var section = _configuration.GetSection("videoExtensions");
            return section.Get<string[]>();
        }

        public string GetImageFolderName()
        {
            var section = _configuration.GetSection("imgFolder");
            return section.Get<string>();
        }

        public string GetVideoFolderName()
        {
            var section = _configuration.GetSection("vidFolder");
            return section.Get<string>();
        }
    }
}
