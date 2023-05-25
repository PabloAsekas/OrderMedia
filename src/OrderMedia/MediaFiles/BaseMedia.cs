using System;
using System.Text.RegularExpressions;
using OrderMedia.Interfaces;

namespace OrderMedia.MediaFiles
{
    /// <summary>
    /// Base media class. Used to be implemented by the rest of the media classes.
    /// </summary>
    public abstract class BaseMedia
    {
        /// <summary>
        /// Gets current media path.
        /// </summary>
        public string MediaPath { get; private set; }

        /// <summary>
        /// Gets current media folder.
        /// </summary>
        public string MediaFolder { get; private set; }

        /// <summary>
        /// Gets classification folder name.
        /// </summary>
        public string ClassificationFolderName { get; private set; }

        /// <summary>
        /// Gets media name with extension included.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Gets media name without extension.
        /// </summary>
        public string NameWithoutExtension {
            get
            {
                return _ioService.GetFileNameWithoutExtension(Name);
            }
        }

        /// <summary>
        /// Gets new media folder.
        /// </summary>
        public string NewMediaFolder { get; private set; }

        /// <summary>
        /// Gets new media location.
        /// </summary>
        public string NewMediaPath { get; private set; }

        /// <summary>
        /// Gets the Created Date Time.
        /// </summary>
        public DateTime CreatedDateTime { get; protected set; }

        /// <summary>
        /// Gets the new renamed name with extension included.
        /// Format yyyy-MM-dd_HH-MM-ss_Name
        /// If Name exceeds 9 characteres, a sorter name is generated randomly.
        /// </summary>
        public string NewName { get; private set; }

        /// <summary>
        /// Gets new media name without extension.
        /// </summary>
        public string NewNameWithoutExtension {
            get
            {
                return _ioService.GetFileNameWithoutExtension(NewName);
            }
        }

        /// <summary>
        /// IIOService.
        /// </summary>
        protected readonly IIOService _ioService;

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseMedia"/> class.
        /// </summary>
        /// <param name="mediaPath">Media path.</param>
        /// <param name="classificationFolderName">Classification folder name where all the types will be located.</param>
        /// <param name="ioService">IIOService injection.</param>
        public BaseMedia(string mediaPath, string classificationFolderName, IIOService ioService)
        {
            MediaPath = mediaPath;
            MediaFolder = _ioService.GetDirectoryName(mediaPath);
            ClassificationFolderName = classificationFolderName;
            Name = _ioService.GetFileName(mediaPath);
            _ioService = ioService;
        }

        public BaseMedia() { }

        /// <summary>
        /// Process logic to clasify the media.
        /// </summary>
        public void Process()
        {
            MoveMedia();

            PostProcess();
        }

        /// <summary>
        /// Process logic after regular clasification is made.
        /// </summary>
        public abstract void PostProcess();

        /// <summary>
        /// Sets creation date.
        /// </summary>
        protected abstract void SetCreationDate();

        private void MoveMedia()
        {
            SetCreationDate();

            string FolderMediaDateName = GetCreationDateAsString("yyyy-MM-dd");

            SetNewName();

            NewMediaFolder = _ioService.Combine(new string[] { MediaFolder, ClassificationFolderName, FolderMediaDateName });
            NewMediaPath = _ioService.Combine(new string[] { NewMediaFolder, NewName });

            _ioService.CreateFolder(NewMediaFolder);
            _ioService.MoveMedia(MediaPath, NewMediaPath);
        }

        private string GetCreationDateAsString(string format)
        {
            return CreatedDateTime.ToString(format);
        }

        private void SetNewName()
        {
            string dateTime = GetCreationDateAsString("yyyy-MM-dd_HH-mm-ss");

            string finalName = dateTime;

            string cleanedName = GetCleanedName(NameWithoutExtension);

            if (cleanedName.Length < 9)
            {
                finalName += $"_{cleanedName}";
            }
            else
            {
                var randomNumber = new Random().Next(0, 9999).ToString("D4");
                finalName += $"_pbg_{randomNumber}";
            }

            NewName = Name.Replace(NameWithoutExtension, finalName);
        }

        private static string GetCleanedName(string name)
        {
            // Remove possible (1), (2), etc. from the name.
            string cleanedName = Regex.Replace(name, @"\([\d]\)", string.Empty);

            // Remove possible start and end spaces.
            cleanedName.Trim();

            return cleanedName;
        }
    }
}
