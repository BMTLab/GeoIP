using System.Runtime.CompilerServices;

using Microsoft.Extensions.Logging;


namespace Shared.Utils.TypeExtensions;

public static class LoggerExtensions
{
    public static void LogEntryMethod(this ILogger logger, [CallerMemberName] string methodName = "") =>
        logger.Log(LogLevel.Trace,  methodName,Array.Empty<object>());
}