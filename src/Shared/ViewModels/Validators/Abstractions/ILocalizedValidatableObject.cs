using FluentValidation.Results;

using Localization;


namespace Shared.ViewModels.Validators.Abstractions;

public interface ILocalizedValidatableObject
{
    public bool IsValid(ILocalization localization, out IReadOnlyCollection<ValidationFailure> errors);
}