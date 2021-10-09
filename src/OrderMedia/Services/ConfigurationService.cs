// <copyright file="ConfigurationService.cs" company="Pablo Bermejo">
// Copyright (c) Pablo Bermejo. All rights reserved.
// </copyright>

namespace OrderMedia.Services
{
    using Microsoft.Extensions.Configuration;
    using OrderMedia.Interfaces;

    /// <summary>
    /// Configuration service class.
    /// </summary>
    public class ConfigurationService : IConfigurationService
    {
        private readonly IConfiguration configuration;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigurationService"/> class.
        /// </summary>
        /// <param name="configuration">Configuration.</param>
        public ConfigurationService(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        /// <inheritdoc/>
        public string[] GetImageExtensions()
        {
            var section = this.configuration.GetSection("imageExtensions");
            return section.Get<string[]>();
        }

        /// <inheritdoc/>
        public string[] GetVideoExtensions()
        {
            var section = this.configuration.GetSection("videoExtensions");
            return section.Get<string[]>();
        }

        /// <inheritdoc/>
        public string[] GetMediaExtensions()
        {
            var section = this.configuration.GetSection("mediaExtensions");
            return section.Get<string[]>();
        }

        /// <inheritdoc/>
        public string GetImageFolderName()
        {
            var section = this.configuration.GetSection("imgFolder");
            return section.Get<string>();
        }

        /// <inheritdoc/>
        public string GetVideoFolderName()
        {
            var section = this.configuration.GetSection("vidFolder");
            return section.Get<string>();
        }
    }
}
