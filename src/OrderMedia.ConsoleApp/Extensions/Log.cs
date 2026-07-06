using Microsoft.Extensions.Logging;

namespace OrderMedia.ConsoleApp.Extensions;

public static partial class Log
{
    [LoggerMessage(Level = LogLevel.Information, Message = "Classification started")]
    public static partial void StartClassification(this ILogger logger);
    
    [LoggerMessage(Level = LogLevel.Information, Message = "Classification ended")]
    public static partial void EndClassification(this ILogger logger);
    
    [LoggerMessage(Level = LogLevel.Information, Message = "Renaming started")]
    public static partial void StartRenaming(this ILogger logger);
    
    [LoggerMessage(Level = LogLevel.Information, Message = "Renaming ended")]
    public static partial void EndRenaming(this ILogger logger);
    
    [LoggerMessage(Level = LogLevel.Information, Message = "Synchronization started")]
    public static partial void StartSynchronization(this ILogger logger);
    
    [LoggerMessage(Level = LogLevel.Information, Message = "Synchronization ended")]
    public static partial void EndSyncrhonization(this ILogger logger);
}