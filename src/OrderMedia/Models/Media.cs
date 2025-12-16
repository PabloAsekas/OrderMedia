using System;
using OrderMedia.Enums;

namespace OrderMedia.Models;

public class Media
{
    /// <summary>
    /// Media Type. This value is used to perform some actions.
    /// Ex. Image, Raw, Video or WhatsApp.
    /// </summary>
    public MediaType Type { get; init; }

    /// <summary>
    /// Media path.
    /// Ex. /pablo/photos/img_0001.heic
    /// </summary>
    public string Path { get; init; } = string.Empty;

    /// <summary>
    /// Media directory path.
    /// Ex. Having a Path "/pablo/photos/img_0001.heic"
    ///     DirectoryPath = "/pablo/photos/"
    /// </summary>
    public string DirectoryPath { get; init; } = string.Empty;

    /// <summary>
    /// Media name, with extension.
    /// Ex. Having a Path "/pablo/photos/img_0001.heic"
    ///     Name = "img_0001.heic"
    /// </summary>
    public string Name { get; init; } = string.Empty;

    /// <summary>
    /// Media name, without extension.
    /// Ex. Having a Path "/pablo/photos/img_0001.heic"
    ///     NameWithoutExtension = "img_0001"
    /// </summary>
    public string NameWithoutExtension { get; init; } = string.Empty;   

    /// <summary>
    /// Created Date Time, when the media was captured based on different scenarios (Metadata, name, etc.).
    /// </summary>
    public DateTimeOffset CreatedDateTime { get; init; }
}