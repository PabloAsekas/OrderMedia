# Order Media

![Order Media](https://github.com/PabloAsekas/OrderMedia/assets/2021846/f46b7ba6-0302-46a6-b67f-bcc77240d61e)

Order Media is a .NET 8 console application made to classify media files based on the captured date.

To have an ordered media catalog, this application will move each file to the corresponding folder based on the captured date.

The default name of the files produced by cameras or phones is arbitrary and useless, the application will rename each file adding the captured date to the name. It will replace the original name if it is too long with a shorter one if configured.

## How it works

1. Gets all the media files.
2. Extracts the captured date.
3. Renames the media with the format `yyyy-MM-dd_originalName`. The `originalName` can be replaced by a random name with random numbers if the `originalName` lenght exceeds the configured limit. The rename behavior and the new name can be configured.
3. Creates a folder with the captured date (format `yyyy-MM-dd`) and moves the media to the folder.

## Supported media

The application supports the following formats:

Image:
- ARW
- CR2
- CR3
- DNG
- Gif
- Heic
- Jpeg
- Jpg
- PNG
- Insp

Video:
- Mov
- Mp4
- Insv

It also supports:

- **LivePhotos:** if a `.heic` or `.jpg` file is a LivePhoto, the application will move and rename the video. Compatible video formats: `.mov` and `.mp4`.
- **Aae files:** sometimes, Apple photos (like Portrait photos) export an `.aae` file with metadata related to the Portrait effect. The application will move and rename the file too.
- **Xmp files:** For raw media files, sometimes there is a sidecar file with `.xmp` extension. This file contains metadata about the photo.
    - The application will move and rename the file.
    - The application will get the Created date from the file and not the raw file. It is like this because when you update the date of a raw file, it doesn't update the main file, it changes the XMP file.
- **WhatsApp media:** if you share a photo or video from the iPhone to the Mac with AirDrop, the file will be called `PHOTO-yyyy-MM-dd-HH-mm-ss`, `VIDEO-yyyy-MM-dd-HH-mm-ss` or `GIF-yyyy-MM-dd-HH-mm-ss`. The application will extract the created date from this name and perform the classification. ~~It will also add the extracted date to the metadata of the photo so every other program will detect it automatically.~~
- **Insta360 media:** including 360 photos, 360 raw photos, 360 videos and 360 preview files. The application will extract the created date from the name and will rename the media files respecting the required naming convention of Insta360 Studio app.
- **NextCloud media:** uploading media to NextCloud from mobile apps will end up with a new name for the media. The application will get the captured date from that name and perform the needed renaming.

## Configuration

The application can be configured through the `appsettings.json` file:

- `MediaSourcePath` *(string)*: The source folder where all the media to be classified is.
- `ImageFolderName` *(string)*: Name of the folder where all the classified images will be located.
- `VideoFolderName` *(string)*: Name of the folder where all the classified videos will be located.
- `ImageExtensions` *(string array)*: Image extensions list the application will check for. If a compatible extension is not specified, the application will not read those files.
- `VideoExtensions` *(string)*: Video extensions list the application will check for.  If a compatible extension is not specified, the application will not read those files.
- `OverwriteFiles` *(boolena)*: Setting to allow overwrite files if the media type is already in the destination.
- `RenameMediaFiles` *(boolean)*: Setting to allow rename media files with the created date or not. Ex: From `IMG_0001,heic` to `yyyy-MM-dd_HH-mm:ss_IMG_0001.heic`.
- `ReplaceLongNames` *(boolean)*: Setting to allow replace the original long names with another one (composed by the setting `NewMediaName` and a 4-digit random number) when the original name is higher than the `MaxMediaNameLength` setting.
- `MaxMediaNamelength` *(integer)*: Maximum media name length to replace the name. If a media name length is higher than this setting, the name will be replaced with a concatenation of the setting `NewMediaName` and a 4-digit random number.
- `NewMediaName` *(string)*: New media name to replace original names when the conditions are met. This name will be concatenated to a 4-digit random number. Ex: From `IMG_0001.heic` to `pbg_3107.heic`.
- `MediaPostProcessPath` *(string)*: The folder where all the exported photos are located.
- `MediaPostProcessSource` *(string)*: The folder where the original photos and files are located.

## Credits

This application could not be possible without the amazing job performed by:

- [MetadataExtractor](https://github.com/drewnoakes/metadata-extractor-dotnet): used to get the metadata of the files.
- [ImageSharp](https://github.com/SixLabors/ImageSharp): used to modify image's metadata.
- [XMP Core](https://github.com/drewnoakes/xmp-core-dotnet/): used to interact with XMP files.