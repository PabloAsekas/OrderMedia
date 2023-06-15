# Order Media

Order Media is a .NET 6 console application made to classify media files based on the captured date.

To have an ordered media catalog, this application will move each file to the corresponding folder based on the captured date.

The default name of the files produced by cameras or phones are arbitrary and useless, the application will rename each file adding the captured date to the name. It will replace the original name if it is too long with a shorter one.

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

- LivePhoto: if a `.heic` or `.jpg` file is a LivePhoto, the application will move and rename the video.
- Aae files: sometimes, Apple photos (like Portrait photos) export a `.aae` file with metadata related to the Portrait effect. The application will move and rename the file.
- WhatsApp media:  if you share a photo or video from the iPhone to the Mac with AirDrop, the file will be called `PHOTO-yyyy-MM-dd-HH-mm-ss` or `VIDEO-yyyy-MM-dd-HH-mm-ss`. The application will extract the created date from this name and perform the classification.

## Configuration

The application can be configured through the `appsettings.json` file:

- `MediaSourcePath`: The source folder where all the media to be classified is.
- `ImageFolderName`: Name of the folder where all the classified images will be located.
- `VideoFolderName`: Name of the folder where all the classified videos will be located.
- `ImageExtensions`: Image extensions list the application will check for. If a compatible extension is not specified, the application will not read those files.
- `VideoExtensions`: Video extensions list the application will check for.  If a compatible extension is not specified, the application will not read those files.