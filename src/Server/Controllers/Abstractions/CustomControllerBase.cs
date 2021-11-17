using System.Diagnostics.CodeAnalysis;
using System.Net.Mime;

using FluentValidation;

using Localization;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Server.Models.Constants;

using Shared.Constants;
using Shared.ViewModels.Results;
using Shared.ViewModels.Validators.Abstractions;


namespace Server.Controllers.Abstractions;

[Authorize]
[Route(@$"{Urls.Api.Prefix}/[controller]/[action]")]
[Produces(MediaTypeNames.Application.Json)]
[ResponseCache(CacheProfileName = CacheProfileNames.Small)]
[ProducesErrorResponseType(typeof(ErrorResult))]
[SuppressMessage("ReSharper", "ContextualLoggerProblem")]
public abstract class CustomControllerBase<T> : ControllerBase where T : CustomControllerBase<T>
{
    #region Fields
    protected readonly ILocalization L;
    protected readonly ILogger<T>? Logger;
    #endregion


    #region Ctors
    protected CustomControllerBase
    (
        ILocalization localization,
        ILogger<T>? logger = null
    )
    {
        L = localization;
        Logger = logger;

    }
    #endregion
    

    #region Methods
    [MemberNotNullWhen(false)]
    protected private bool IsValidationFailed(ILocalizedValidatableObject model, [NotNullWhen(true)] out IActionResult? returnAction) =>
        !TryValidateModel(model, out returnAction);

        
    protected private bool TryValidateModel(ILocalizedValidatableObject model, [NotNullWhen(false)] out IActionResult? returnAction)
    {
        if (model is null)
            throw new ArgumentNullException(nameof(model));

        if (!model.IsValid(L, out var errors))
        {
            if (errors.Any(e => e.Severity == Severity.Error))
            {
                returnAction = BadRequest(new ErrorResult
                {
                    Errors = errors
                            .Where(e => e.Severity == Severity.Error)
                            .Select(p => p.ErrorMessage)
                });
            }
            else
            {
                returnAction = UnprocessableEntity
                (
                    new ErrorResult
                    {
                        Errors = errors
                                .Where(e => e.Severity != Severity.Error)
                                .Select(p => p.ErrorMessage)
                    }
                );
            }

            return false;
        }

        returnAction = null;

        return true;
    }
    #endregion _Methods
}