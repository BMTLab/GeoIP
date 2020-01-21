#region HEADER
//   ValidateModelAttribute.cs of GeoIP.Server
//   Created by Nikita Neverov at 20.01.2020 9:06
#endregion


using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;


namespace GeoIP.Server.Filters
{
    /// <summary>
    /// ActionFilter. Intercepts model errors and returns bad result with information
    /// </summary>
    public sealed class ValidateRequestAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuted(ActionExecutedContext context)
        {
            if (!context.ModelState.IsValid)
            {
                context.Result = new BadRequestObjectResult(context.ModelState);
            }
        }
    }
}