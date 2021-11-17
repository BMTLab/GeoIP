using System.Collections.Concurrent;
using System.Text;

using JetBrains.Annotations;

using NLog.Config;
using NLog.LayoutRenderers;


namespace NLog.GeoIP;

/// <summary>
/// <para>Renders exception starting from new line with short type exception name followed by message and stacktrace (optionally)</para>
/// <para>If exception is logged more than once (catched, logged and re-thrown as inner), stack trace is not written </para>
/// </summary>
[LayoutRenderer("indent-exception")]
[ThreadAgnostic]
// [ThreadSafe]
public sealed class IndentExceptionLayoutRenderer : LayoutRenderer
{
    /// <summary>
    ///     Holds logged already exceptions just to skip stack logging
    /// </summary>
    public static readonly ConcurrentQueue<Exception> LoggedErrors = new();


    #region Ctor
    public IndentExceptionLayoutRenderer()
    {
        Indent = "\t";
        StackTraceIndent = "\t\t";
        BeforeType = "[";
        AfterType = "]";
        LogStack = true;
        Separator = @" ";
    }
    #endregion _Ctor


    #region Properties
    /// <summary>
    ///     Indent before exception type (default is tab)
    /// </summary>
    [PublicAPI]
    public string Indent { get; set; }
        
        
    /// <summary>
    ///     Indent between each stack trace line (default is two tab characters)
    /// </summary>
    [PublicAPI]
    public string StackTraceIndent { get; set; }
        
        
    /// <summary>
    ///     Is written before exception type name (default [)
    /// </summary>
    [PublicAPI]
    public string BeforeType { get; set; }
        
        
    /// <summary>
    ///     Is written after exception type name (default ])
    /// </summary>
    [PublicAPI]
    public string AfterType { get; set; }
        
        
    /// <summary>
    ///     Separator between exception type and message
    /// </summary>
    [PublicAPI]
    public string Separator { get; set; }
        
        
    /// <summary>
    ///     Log stack trace or not (for console logger e.g.)
    /// </summary>
    [PublicAPI]
    public bool LogStack { get; set; }
    #endregion _Properties


    protected override void Append(StringBuilder builder, LogEventInfo logEvent)
    {
        if (builder is null || logEvent is null)
            throw new ArgumentNullException(nameof(logEvent));

        var exc = logEvent.Exception;

        while (exc != null)
        {
            builder.AppendFormat(@"{1}{2}{0}{3}{4}", exc.GetType().Name, Indent, BeforeType, AfterType, Separator);
            builder.Append(exc.Message);

            if (LogStack)
            {
                var stackTraceWasLogged = LoggedErrors.Contains(exc);
                var stackTrace = exc.StackTrace;

                if (!stackTraceWasLogged && stackTrace != null)
                {
                    builder.AppendLine();
                    LoggedErrors.Enqueue(exc);
                    builder.AppendFormat("{0}", stackTrace.Replace(@"   ", StackTraceIndent, StringComparison.Ordinal));
                }

                if (LoggedErrors.Count > 33)
                {
                    LoggedErrors.TryDequeue(out var _);
                    LoggedErrors.TryDequeue(out var _);
                }
            }

            exc = exc.InnerException;

            if (exc != null)
                builder.AppendLine();
        }
    }
}