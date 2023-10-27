# Order Media

![Order Media](https://github.com/PabloAsekas/OrderMedia/assets/2021846/f46b7ba6-0302-46a6-b67f-bcc77240d61e)

Order Media is a .NET 6 console application made to classify media files based on the captured date.

To have an ordered media catalog, this application will move each file to the corresponding folder based on the captured date.

The default name of the files produced by cameras or phones are arbitrary and useless, the application will rename each file adding the captured date to the name. It will replace the original name if it is too long with a shorter one if configured.

## How it works

1. Gets all the media files.
2. Extracts the captured date.
3. Renames the media with the format `yyyy-MM-dd_originalName`. If the name is bigger than 9 characters, it creates a new random name.
3. Creates a folder with the captured date (format `yyyy-MM-dd`) and moves the media.

## Supported media

The application supports the following formats:

Image:
- ARW
- DNG
- Gif
- Heic
- Jpeg
- Jpg
- PNG

Video:
- Mov
- Mp4

It also supports:

- LivePhoto: if a `.heic` or `.jpg` file is a LivePhoto, the application will move and rename the video. Compatible video formats: `.mov` and `.mp4`.
- Aae files: sometimes, Apple photos (like Portrait photos) export a `.aae` file with metadata related to the Portrait effect. The application will move and rename the file.
- Xmp files: For raw media files, sometimes there is a sidecar file with `.xmp` extension. This file contains metadata about the photo.
    - The application will move and rename the file.
    - The application will get the Created date from the file and not the raw file.
- WhatsApp media:  if you share a photo or video from the iPhone to the Mac with AirDrop, the file will be called `PHOTO-yyyy-MM-dd-HH-mm-ss` or `VIDEO-yyyy-MM-dd-HH-mm-ss`. The application will extract the created date from this name and perform the classification.

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
- `NewMediaName` *(string)*: New media name to replace original names when the conditions are met. This name will be concatenated to a 4-digit random number. Ex: From `IMG_0001.heic` to `pbg_3107.heic`.
- `MaxMediaNamelength` *(integer)*: Maximum media name length to replace the name. If a media name length is higher than this setting, the name will be replaced with a concatenation of the setting `NewMediaName` and a 4-digit random number.
