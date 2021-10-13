// <copyright file="WhatsAppMedia.cs" company="Pablo Bermejo">
// Copyright (c) Pablo Bermejo. All rights reserved.
// </copyright>

namespace OrderMedia.MediaFiles
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Text.RegularExpressions;
    using MetadataExtractor;
    using MetadataExtractor.Formats.Exif;

    /// <summary>
    /// Media class for WhatsApp media files.
    /// </summary>
    public class WhatsAppMedia : BaseMedia
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WhatsAppMedia"/> class.
        /// </summary>
        /// <param name="path">Media path.</param>
        /// <param name="classificationFolderName">Classification folder name where all the types will be located.</param>
        public WhatsAppMedia(string path, string classificationFolderName)
            : base(path, classificationFolderName)
        {
        }

        /// <inheritdoc/>
        public override string GetCreationDate()
        {
            string pattern = @"[0-9]{4}-(0[1-9]|1[0-2])-(0[1-9]|[1-2][0-9]|3[0-1])";

            Match m = Regex.Match(this.Name, pattern, RegexOptions.IgnoreCase);

            // We assume that the regex will succeed.
            return m.Value;
        }
    }
}
