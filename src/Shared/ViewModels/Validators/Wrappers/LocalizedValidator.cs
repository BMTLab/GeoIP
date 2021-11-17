using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;

using FluentValidation;
using FluentValidation.Results;


namespace Shared.ViewModels.Validators.Wrappers;


internal static class LocalizedValidator
{
    [MemberNotNullWhen(true)]
    internal static bool IsValid<T>([NotNullWhen(true)] T model, IValidator<T> validator, out IReadOnlyCollection<ValidationFailure> errors)
    {
        var result = validator.Validate(model);

        if (result.IsValid)
        {
            errors = ImmutableList<ValidationFailure>.Empty;

            return true;
        }

        errors = result.Errors;

        return false;
    }
}