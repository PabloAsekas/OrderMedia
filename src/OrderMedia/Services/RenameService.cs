using System;
using System.Text.RegularExpressions;
using OrderMedia.Interfaces;

namespace OrderMedia.Services
{
    /// <summary>
    /// Rename Service.
    /// </summary>
    public class RenameService : IRenameService
    {
        private readonly IIOService _ioService;
        private readonly IRandomizerService _randomizerService;
        private readonly IConfigurationService _configurationService;

        public RenameService(IIOService ioService, IRandomizerService randomizerService, IConfigurationService configurationService)
        {
            _ioService = ioService;
            _randomizerService = randomizerService;
            _configurationService = configurationService;
        }

        public string Rename(string name, DateTime createdDateTime)
        {
            string finalName = createdDateTime.ToString("yyyy-MM-dd_HH-mm-ss");

            string extension = _ioService.GetExtension(name);

            string cleanedName = GetCleanedName(name);

            if (ReplaceName(cleanedName.Length))
            {
                var randomNumber = _randomizerService.GetRandomNumberAsD4();
                finalName += $"_{_configurationService.GetNewMediaName()}_{randomNumber}";
            }
            else
            {
                finalName += $"_{cleanedName}";
            }

            return $"{finalName}{extension}";
        }

        public string GetAaeName(string nameWithoutExtension)
        {
            // Images with the (1) have the aae as IMG_xxxx (1)O.aae 
            if (nameWithoutExtension.Contains('('))
            {
                return $"{nameWithoutExtension}O.aae";
            }

            // Images with regular names have the aae as IMG_Oxxx.aae
            return $"{nameWithoutExtension.Insert(4, "O")}.aae";
        }

        private string GetCleanedName(string name)
        {
            // Remove extension.
            string cleanedName = _ioService.GetFileNameWithoutExtension(name);

            // Remove possible (1), (2), etc. from the name.
            cleanedName = Regex.Replace(cleanedName, @"\([\d]\)", string.Empty);

            // Remove possible start and end spaces.
            cleanedName = cleanedName.Trim();

            return cleanedName;
        }

        private bool ReplaceName(int cleanedNameLength) {
            return cleanedNameLength > _configurationService.GetMaxMediaNameLength() && _configurationService.GetReplaceLongNames();
        }
    }
}

