using System;
using System.IO;
using OrderMedia.Interfaces;
using OrderMedia.Services;

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
        public string NameWithoutExtension { get; private set; }

        /// <summary>
        /// Gets new media folder.
        /// </summary>
        public string NewMediaFolder { get; private set; }

        /// <summary>
        /// Gets new media location.
        /// </summary>
        public string NewMediaPath { get; private set; }

        public DateTime CreatedDateTime { get; protected set; }

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
            MediaFolder = Path.GetDirectoryName(mediaPath);
            ClassificationFolderName = classificationFolderName;
            Name = Path.GetFileName(mediaPath);
            NameWithoutExtension = Path.GetFileNameWithoutExtension(mediaPath);
            _ioService = ioService;
        }

        /// <summary>
        /// Gets creation date.
        /// </summary>
        /// <returns>Creation date string in format yyyy-MM-dd.</returns>
        public abstract string GetCreationDate();

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

        private void MoveMedia()
        {
            string mediaDate = GetCreationDate();

            NewMediaFolder = Path.Combine(MediaFolder, ClassificationFolderName, mediaDate);
            NewMediaPath = Path.Combine(NewMediaFolder, Name);

            _ioService.CreateFolder(NewMediaFolder);
            _ioService.MoveMedia(MediaPath, NewMediaPath);
        }
    }
}
