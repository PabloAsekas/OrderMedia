using OrderMedia.Interfaces;

namespace OrderMedia.Services
{
    /// <summary>
    /// Rename Service.
    /// </summary>
    public class AaeHelperService : IAaeHelperService
    {
        public string GetAaeName(string nameWithoutExtension)
        {
            // Images with no proper name, return the same name.aae
            if (nameWithoutExtension.Length < 5)
            {
                return $"{nameWithoutExtension}.aae";
            }

            // Images with the (1) have the aae as IMG_xxxx (1)O.aae 
            if (nameWithoutExtension.Contains('('))
            {
                return $"{nameWithoutExtension}O.aae";
            }

            // Images with regular names have the aae as IMG_Oxxxx.aae
            return $"{nameWithoutExtension.Insert(4, "O")}.aae";
        }
    }
}

