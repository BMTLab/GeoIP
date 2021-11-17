using System.Diagnostics.CodeAnalysis;


namespace Server.Middlewares.Extensions;


[SuppressMessage("ReSharper", "UnusedMethodReturnValue.Global")]
public static class MiddlewareExtensions
{
    public static IApplicationBuilder UseServerLocalization(this IApplicationBuilder builder)
    {
        builder.UseMiddleware<LocalizationMiddleware>();

        return builder;
    }
}