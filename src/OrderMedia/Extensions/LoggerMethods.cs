using Microsoft.Extensions.Logging;

namespace OrderMedia.Extensions;

public static partial class LoggerMethods
{
    [LoggerMessage(Level = LogLevel.Error, Message = "Error reading metadata from '{mediaPath}'")]
    public static partial void ErrorReadingMetadata(this ILogger logger, string mediaPath);
}