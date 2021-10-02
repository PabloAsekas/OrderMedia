// <copyright file="IOService.cs" company="Pablo Bermejo">
// Copyright (c) Pablo Bermejo. All rights reserved.
// </copyright>

namespace OrderMedia.Services
{
    using System.Collections.Generic;
    using System.IO;
    using OrderMedia.Extensions;
    using OrderMedia.Interfaces;

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
        public void MoveMedia(FileInfo file, string subFolderName, string type) //TODO: refactorizar el type
        {
            var folder = type == "i" ? this.imgFolder : this.vidFolder;
            string subFolderFullPath = Path.Combine(this.path, folder, subFolderName); // TODO: Averiguar cuando es una img o un vid.
            this.CreateFolder(subFolderFullPath);

            string newMediaLocation = Path.Combine(subFolderFullPath, file.Name);
            this.MoveMedia(file.FullName, newMediaLocation);

            this.MoveLivePhoto(file, subFolderFullPath);
        }

        private void CreateFolder(string path)
        {
            Directory.CreateDirectory(path);
        }

        private void MoveLivePhoto(FileInfo file, string folder)
        {
            string imageNameWithoutExtension = Path.GetFileNameWithoutExtension(file.FullName);
            string videoName = $"{imageNameWithoutExtension}.mov";
            string videoLocation = Path.Combine(this.path, videoName);

            if (File.Exists(videoLocation))
            {
                string newVideoLocation = Path.Combine(folder, videoName);
                this.MoveMedia(videoLocation, newVideoLocation);
            }
        }

        private void MoveMedia(string oldPath, string newPath)
        {
            File.Move(oldPath, newPath);
        }
    }
}
