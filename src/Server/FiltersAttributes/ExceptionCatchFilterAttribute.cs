using System.Net;

using JetBrains.Annotations;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

using Server.Models.Constants;

using Shared.ViewModels.Results;


namespace Server.FiltersAttributes;

[UsedImplicitly]
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public sealed class ExceptionCatchFilterAttribute : Attribute, IExceptionFilter
{
    #region Ctors
    public ExceptionCatchFilterAttribute
    (
        ILogger<ExceptionCatchFilterAttribute> logger
    )
    {
        Logger = logger;
    }
    #endregion

        
    #region Properties
    public ILogger<ExceptionCatchFilterAttribute> Logger { get; }
    #endregion _Properties

        
    #region Methods
    public void OnException(ExceptionContext context)
    {
        var actionInfo = context.ActionDescriptor.RouteValues;
        var exception = context.Exception;
        var innerException = exception.InnerException;
        var isInnerException = innerException != null;
            
        var innerOutput =
            isInnerException ? "Has inner exception" : "No inner exceptions";
        
        Logger.LogError(exception, $"In action {actionInfo} an exception was thrown.\n {exception.StackTrace}.\n {exception.Data} \n {innerOutput}");
            
        context.Result = new ObjectResult
        (
            new ErrorResult(NeutralMessages.ServerError)
        )
        {
            StatusCode = (int) HttpStatusCode.InternalServerError,
            DeclaredType = typeof(ErrorResult)
        };

        context.ExceptionHandled = true;
    }
    #endregion
}