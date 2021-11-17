using System.Diagnostics.CodeAnalysis;

using FluentValidation.Results;

using Localization;

using Shared.ViewModels.Validators;
using Shared.ViewModels.Validators.Abstractions;
using Shared.ViewModels.Validators.Wrappers;


namespace Shared.ViewModels;


public record IpAddressModel : ILocalizedValidatableObject
{
    public string? Ip { get; init; }
        

    #region Implementation of ILocalizedValidatableObject
    [MemberNotNullWhen(true, nameof(Ip))]
    public virtual bool IsValid(ILocalization localization, out IReadOnlyCollection<ValidationFailure> errors) =>
        LocalizedValidator.IsValid(this, new IpAddressModelValidator(localization), out errors);
    #endregion
}