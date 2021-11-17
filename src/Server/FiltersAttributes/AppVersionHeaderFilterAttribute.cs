using JetBrains.Annotations;

using Microsoft.AspNetCore.Mvc.Filters;

using Server.Settings;

using Shared.Constants;


namespace Server.FiltersAttributes;


[UsedImplicitly]
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public sealed class AppVersionHeaderFilterAttribute : Attribute, IAlwaysRunResultFilter
{
    #region Methods
    public void OnResultExecuted(ResultExecutedContext context)
    {
        // Method should be empty
    }


    public void OnResultExecuting(ResultExecutingContext context)
    {
        context.HttpContext.Response.Headers.Add(CustomHeaderNames.XVersion, GeoIpEnvironment.ServerVersion);
    }
    #endregion
}