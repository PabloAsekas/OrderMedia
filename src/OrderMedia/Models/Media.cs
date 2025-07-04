using System;
using OrderMedia.Enums;

namespace OrderMedia.Models;

public class Media
{
    /// <summary>
    /// Gets or sets the media type.
    /// Ex. Image, Raw, Video or WhatsApp.
    /// </summary>
    public MediaType MediaType { get; set; }

    /// <summary>
    /// Gets or sets current media path.
    /// Ex. /pablo/photos/img_0001.heic
    /// </summary>
    public string MediaPath { get; set; }

    /// <summary>
    /// Gets or sets current media folder name.
    /// Ex. Having a MediaPath "/pablo/photos/img_0001.heic"
    ///     MediaFolder = "/pablo/photos/"
    /// </summary>
    public string MediaFolder { get; set; }

    /// <summary>
    /// Gets or sets media name with extension included.
    /// Ex. Having a MediaPath "/pablo/photos/img_0001.heic"
    ///     Name = "img_0001.heic"
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets media name without extension.
    /// Ex. Having a MediaPath "/pablo/photos/img_0001.heic"
    ///     NameWithoutExtension = "img_0001"
    /// </summary>
    public string NameWithoutExtension { get; set; }

    /// <summary>
    /// Gets or sets the Created DateTimeOffset.
    /// </summary>
    public DateTimeOffset CreatedDateTimeOffset { get; set; }

    /// <summary>
    /// Gets or sets the new renamed name with extension included.
    /// Ex. Having a name img_0001.heic and a CreatedDateTime 2014-07-31 22:15:15
    ///     NewName = "2014-07-31_22-15-15_img_0001.heic"
    /// </summary>
    public string NewName { get; set; }

    /// <summary>
    /// Gets or sets new media name without extension.
    /// Ex. Having a name img_0001.heic and a CreatedDateTime 2014-07-31 22:15:15
    ///     NewNameWithoutExtension = "2014-07-31_22-15-15_img_0001"
    /// </summary>
    public string NewNameWithoutExtension { get; set; }

    /// <summary>
    /// Gets or sets new media folder.
    /// Ex. Having a MediaPath "/pablo/photos/img_0001.heic" and a Configuration.GetImageFolderName "img"
    ///     NewMediaFolder = "/pablo/photos/img/2014-07-31/"
    /// </summary>
    public string NewMediaFolder { get; set; }

    /// <summary>
    /// Gets or sets new media location.
    /// Ex. Having a NewNAme "2014-07-31_22-15-15_img_0001.heic" and NewMediaFolder = "/pablo/photos/order/img/2014-07-31/"
    ///     NewMediaPath = "/pablo/photos/img/2014-07-31/2014-07-31_22-15-15_img_0001.heic"
    /// </summary>
    public string NewMediaPath { get; set; }        
}