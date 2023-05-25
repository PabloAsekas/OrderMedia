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
        protected string MediaPath { get; private set; }

        /// <summary>
        /// Gets classification folder name.
        /// </summary>
        protected string ClassificationFolderName { get; private set; }

        /// <summary>
        /// IIOService.
        /// </summary>
        protected readonly IIOService _ioService;

        /// <summary>
        /// Gets current media folder.
        /// </summary>
        protected string MediaFolder {
            get
            {
                return _ioService.GetDirectoryName(MediaPath);
            }
        }

        /// <summary>
        /// Gets media name with extension included.
        /// </summary>
        protected string Name {
            get
            {
                return _ioService.GetFileName(MediaPath);
            }
        }

        /// <summary>
        /// Gets media name without extension.
        /// </summary>
        protected string NameWithoutExtension {
            get
            {
                return _ioService.GetFileNameWithoutExtension(Name);
            }
        }

        private DateTime createdDateTime { get; set; }

        /// <summary>
        /// Gets or sets the Created Date Time.
        /// </summary>
        protected DateTime CreatedDateTime {
            get
            {            
                if (createdDateTime == DateTime.MinValue)
                {
                    SetCreationDate();
                }

                return createdDateTime;
            }
            set
            {
                createdDateTime = value;
            }
        }

        private string newMediaPath { get; set; }

        /// <summary>
        /// Gets or sets new media location.
        /// </summary>
        protected string NewMediaPath {
            get
            {
                if (string.IsNullOrEmpty(newMediaPath))
                {
                    SetNewMediaPath();
                }

                return newMediaPath;
            }
            set
            {
                newMediaPath = value;
            }
        }

        private string newMediaFolder { get; set; }

        /// <summary>
        /// Gets or sets new media folder.
        /// </summary>
        protected string NewMediaFolder
        {
            get
            {
                if (string.IsNullOrEmpty(newMediaFolder))
                {
                    SetNewMediaFolder();
                }

                return newMediaFolder;
            }
            set
            {
                newMediaFolder = value;
            }
        }

        private string newName { get; set; }

        /// <summary>
        /// Gets the new renamed name with extension included.
        /// Format yyyy-MM-dd_HH-MM-ss_Name
        /// If Name exceeds 9 characteres, a sorter name is generated randomly.
        /// </summary>
        protected string NewName
        {
            get
            {
                if (string.IsNullOrEmpty(newName))
                {
                    SetNewName();
                }

                return newName;
            }
            set
            {
                newName = value;
            }
        }

        /// <summary>
        /// Gets new media name without extension.
        /// </summary>
        protected string NewNameWithoutExtension {
            get
            {
                return _ioService.GetFileNameWithoutExtension(NewName);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseMedia"/> class.
        /// </summary>
        /// <param name="mediaPath">Media path.</param>
        /// <param name="classificationFolderName">Classification folder name where all the types will be located.</param>
        /// <param name="ioService">IIOService injection.</param>
        public BaseMedia(string mediaPath, string classificationFolderName, IIOService ioService)
        {
            _ioService = ioService;
            MediaPath = mediaPath;
            ClassificationFolderName = classificationFolderName;
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
        protected abstract void PostProcess();

        /// <summary>
        /// Sets creation date.
        /// </summary>
        protected abstract void SetCreationDate();

        private void MoveMedia()
        {
            _ioService.CreateFolder(NewMediaFolder);
            _ioService.MoveMedia(MediaPath, NewMediaPath);
        }

        private void SetNewMediaPath()
        {
            NewMediaPath = _ioService.Combine(new string[] { NewMediaFolder, NewName });
        }

        private void SetNewMediaFolder()
        {
            string mediaFolderDateName = GetCreationDateAsString("yyyy-MM-dd");

            NewMediaFolder = _ioService.Combine(new string[] { MediaFolder, ClassificationFolderName, mediaFolderDateName });
        }

        private void SetNewName()
        {
            string finalName = GetFinalName();

            NewName = Name.Replace(NameWithoutExtension, finalName);
        }

        private string GetCreationDateAsString(string format)
        {
            return CreatedDateTime.ToString(format);
        }

        private string GetFinalName()
        {
            string finalName = GetCreationDateAsString("yyyy-MM-dd_HH-mm-ss");

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

            return finalName;
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
