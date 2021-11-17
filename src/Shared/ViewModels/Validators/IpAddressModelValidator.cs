using FluentValidation;

using Localization;


namespace Shared.ViewModels.Validators;


public sealed class IpAddressModelValidator : AbstractValidator<IpAddressModel>
{
    public IpAddressModelValidator(ILocalization lc)
    {
        RuleFor(p => p.Ip)
           .NotEmpty()
           .WithName("IP");

        RuleFor(p => p.Ip)
           .Matches(@"^((25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.){3}(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)$")
           .WithMessage(lc.ErrorIpIsNotCorrect)
           .WithSeverity(Severity.Warning);
    }
}