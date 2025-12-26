using Microsoft.Extensions.Logging;

namespace OrderMedia.ConsoleApp.Extensions;

public static partial class Log
{
    [LoggerMessage(Level = LogLevel.Information, Message = "Classification started")]
    public static partial void StartClassification(this ILogger logger);
    
    [LoggerMessage(Level = LogLevel.Information, Message = "Classification ended")]
    public static partial void EndClassification(this ILogger logger);
}