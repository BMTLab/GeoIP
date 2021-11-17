using System.ComponentModel;

using JetBrains.Annotations;


namespace Localization;

[PublicAPI]
public sealed record LocalizationOptions
{
    [DefaultValue("en")]
    public string DefaultLanguage { get; set; } = @"en";
    public bool RaiseLocalizationChangedEvent { get; set; }
}