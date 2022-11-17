// <copyright file="IOService.cs" company="Pablo Bermejo">
// Copyright (c) Pablo Bermejo. All rights reserved.
// </copyright>

namespace OrderMedia.Services
{
    using System.Collections.Generic;
    using System.IO;
    using OrderMedia.Extensions;
    using OrderMedia.Interfaces;
    using OrderMedia.MediaFiles;

    /// <summary>
    /// IO service class.
    /// </summary>
    public class IOService : IIOService
    {
        private readonly string path;
        private readonly DirectoryInfo directory;
        private readonly string imgFolder;
        private readonly string vidFolder;

        /// <summary>
        /// Initializes a new instance of the <see cref="IOService"/> class.
        /// </summary>
        /// <param name="path">Path.</param>
        /// <param name="imgFolder">Image folder name.</param>
        /// <param name="vidFolder">Video folder name.</param>
        public IOService(string path, string imgFolder, string vidFolder)
        {
            this.path = path;
            this.directory = new DirectoryInfo(this.path);
            this.imgFolder = imgFolder;
            this.vidFolder = vidFolder;
        }

        /// <inheritdoc/>
        public void CreateMediaFolders()
        {
            this.CreateFolder(Path.Combine(this.path, this.imgFolder));
            this.CreateFolder(Path.Combine(this.path, this.vidFolder));
        }

        /// <inheritdoc/>
        public IEnumerable<FileInfo> GetFilesByExtensions(params string[] extensions)
        {
            return this.directory.GetFilesByExtensions(extensions);
        }

        /// <inheritdoc/>
        public void MoveMedia(BaseMedia media, string subFolderName)
        {
            string subFolderFullPath = Path.Combine(this.path, media.ClassificationFolderName, subFolderName);
            this.CreateFolder(subFolderFullPath);

            string newMediaLocation = Path.Combine(subFolderFullPath, media.Name);
            this.MoveMedia(media.Path, newMediaLocation);

            this.MoveLivePhoto(media.NameWithoutExtension, subFolderFullPath);
        }

        private void CreateFolder(string path)
        {
            Directory.CreateDirectory(path);
        }

        private void MoveLivePhoto(string fileNameWithoutExtension, string folder)
        {
            string videoName = $"{fileNameWithoutExtension}.mov";
            string videoLocation = Path.Combine(this.path, videoName);

            if (File.Exists(videoLocation))
            {
                string newVideoLocation = Path.Combine(folder, videoName);
                this.MoveMedia(videoLocation, newVideoLocation);
            }

            string aaeName = $"{fileNameWithoutExtension}.aae";
            string aaeLocation = Path.Combine(this.path, aaeName);

            if (File.Exists(aaeLocation))
            {
                string newAaeLocation = Path.Combine(folder, aaeName);
                this.MoveMedia(aaeLocation, newAaeLocation);
            }
        }

        private void MoveMedia(string oldPath, string newPath)
        {
            File.Move(oldPath, newPath);
        }
    }
}
