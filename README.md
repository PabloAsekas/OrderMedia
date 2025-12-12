# Order Media

![Order Media](https://github.com/PabloAsekas/OrderMedia/assets/2021846/f46b7ba6-0302-46a6-b67f-bcc77240d61e)

Order Media is a .NET 10 console application made to classify media files based on the captured date.

To have an ordered media catalog, this application will move each file to the corresponding folder based on the captured date.

The default name of the files produced by cameras or phones is arbitrary and useless, the application will rename each file adding the captured date to the name. It will replace the original name if it is too long with a shorter one if configured.

## How it works

1. Gets all the media files.
2. Extracts the captured date from different sources depending on the media file.
3. Renames the media with the format `yyyy-MM-dd_originalName`. The `originalName` can be replaced by a random name with random numbers if the `originalName` lenght exceeds the configured limit. The rename behavior and the new name can be configured.
4. Creates a folder with the captured date (format `yyyy-MM-dd`) and moves the media to the folder.

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
- **Aae files:** sometimes, Apple photos (like Portrait photos) include a `.aae` file with metadata related to the Portrait effect. The application will move and rename the file too.
- **Xmp files:** For raw media files, sometimes there is a sidecar file with `.xmp` extension. This file contains metadata about the photo.
    - The application will move and rename the file.
    - The application will get the Created date from the file and not the raw file. It is like this because when you update the date of a raw file, it doesn't update the main file, it changes the XMP file.
- **WhatsApp media:** if you share a photo or video from the iPhone to the Mac with AirDrop, the file will be called `PHOTO-yyyy-MM-dd-HH-mm-ss`, `VIDEO-yyyy-MM-dd-HH-mm-ss` or `GIF-yyyy-MM-dd-HH-mm-ss`. The application will extract the created date from this name and perform the classification. ~~It will also add the extracted date to the metadata of the photo so every other program will detect it automatically.~~
- **Insta360 media:** including 360 photos, 360 raw photos, 360 videos and 360 preview files. The application will extract the created date from the name and will rename the media files respecting the required naming convention of Insta360 Studio app.
- **NextCloud media:** uploading media to NextCloud from mobile apps will end up with a new name for the media. The application will get the captured date from that name and perform the renaming.
- **Canon M01 XML:** For video files produced with newer Canon cameras, sometimes there is a sidecar file with `.xml` extension. This file contains metadata about the video.
  - The application will move and rename the file.
  - The application will get the Created date from the file and not the video file. It is like this because the video file can miss the offset of the date, resulting in a wrong date.

## Configuration

The application can be configured through the `appsettings.json` file, or environment variables:

- `MediaPaths__MediaSourcePath` *(string)*: The source folder where all the media to be classified is.
- `ClassificationFolders__ImageFolderName` *(string)*: Name of the folder where all the classified images will be located.
- `ClassificationFolders__VideoFolderName` *(string)*: Name of the folder where all the classified videos will be located.
- `MediaExtensions__ImageExtensions` *(string array)*: Image extensions list the application will check for. If a compatible extension is not specified, the application will not read those files.
- `MediaExtensions__VideoExtensions` *(string)*: Video extensions list the application will check for.  If a compatible extension is not specified, the application will not read those files.
- `ClassificationSettings__OverwriteFiles` *(boolena)*: Setting to allow overwriting files if the media type is already in the destination.
- `ClassificationSettings__RenameMediaFiles` *(boolean)*: Setting to allow renaming media files with the created date or not. Ex: From `IMG_0001,heic` to `yyyy-MM-dd_HH-mm:ss_IMG_0001.heic`.
- `ClassificationSettings__ReplaceLongNames` *(boolean)*: Setting to allow replacing the original long names with another one (composed by the setting `NewMediaName` and a 4-digit random number) when the original name is higher than the `MaxMediaNameLength` setting.
- `ClassificationSettings__MaxMediaNameLength` *(integer)*: Maximum media name length to replace the name. If a media name length is higher than this setting, the name will be replaced with a concatenation of the setting `NewMediaName` and a 4-digit random number.
- `ClassificationSettings__NewMediaName` *(string)*: New media name to replace original names when the conditions are met. This name will be concatenated to a 4-digit random number. Ex: From `IMG_0001.heic` to `pbg_3107.heic`.
- `MediaPaths__MediaPostProcessPath` *(string)*: The folder where all the exported photos are located.
- `MediaPaths__MediaPostProcessSource` *(string)*: The folder where the original photos and files are located.
- `ClassificationProcessors__Processors` *(dictionary string array)*: List of all the processors in order of execution by MediaType. It is configured to work properly. No need to change it.

## Docker Support

Docker can be used to execute the application.

### Build image

Execute the following command in the root of the repository to build the image locally:

```shell
docker build -t ordermedia/consoleapp:dev -f src/OrderMedia.ConsoleApp/Dockerfile .
```

### Create container

There is a [`docker-compose.yaml`](https://github.com/PabloAsekas/OrderMedia/blob/main/Deploy/docker-compose.yaml) file that can be used to create the container.

Execute the following command in the root of the repository to create the container using the environment variables configured in the `docker-compose.yaml` file:

```shell
docker compose -f Deploy/docker-compose.yaml up -d
```

## Credits

This application could not be possible without the amazing job performed by:

- [MetadataExtractor](https://github.com/drewnoakes/metadata-extractor-dotnet): used to get the metadata of the files.
- [ImageSharp](https://github.com/SixLabors/ImageSharp): used to modify image's metadata.
- [XMP Core](https://github.com/drewnoakes/xmp-core-dotnet/): used to interact with XMP files.