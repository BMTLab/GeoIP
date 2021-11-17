using System.Globalization;

using JetBrains.Annotations;

using Localization.Events;


namespace Localization;

[PublicAPI]
public interface ILocalizationBuilder
{
    CultureInfo Culture { get; set; }
    ILocalization Localization { get; set; }
    event EventHandler<LocalizationChangedEventArgs>? Changed;
}